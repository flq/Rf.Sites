using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NHibernate;
using NHibernate.Validator.Exceptions;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
namespace Rf.Sites.Migration
{
  class Program
  {
    public const string DataFolder = @"..\..\..\db data";

    static ISessionFactory factory;

    static void Main()
    {
      try
      {
        var sMaker = new SessionFactoryMaker();
        factory = sMaker.CreateFactory();
        setupSchema(sMaker);
        insertTags();
        insertContent();
      }
      catch (InvalidStateException x)
      {
        Console.WriteLine("Entity validation failed...");
        foreach (var i in x.GetInvalidValues())
          Console.WriteLine("Property {0}, Message {1}", i.PropertyName, i.Message);
      }
      catch (Exception x)
      {
        Console.WriteLine(x.GetType().Name);
        Console.WriteLine(x.Message);
        Console.WriteLine(x.StackTrace);
      }

      Console.WriteLine("Press any key to continue...");
      Console.ReadLine();
    }

    private static void insertContent()
    {
      XmlDocument dHead = getDocument("drp_node.xml");
      var dBody = getDocument("drp_node_revisions.xml");
      var dTagToContent = getDocument("drp_term_node.xml");
      var tagDoc = getDocument("drp_term_data.xml");
      var commentDoc = getDocument("drp_comments.xml");
      var nodeWordDoc = getDocument("drp_nodewords.xml");

      var attachmentDoc = getDocument("drp_files.xml");
      var attachmentRelDoc = getDocument("drp_upload.xml");
      
      using (var session = factory.OpenSession())
      using (var tx = session.BeginTransaction())
      {

        var dbTags = session.CreateCriteria<Tag>().List<Tag>();


        foreach (XmlNode n in dHead.SelectNodes("//drp_node"))
        {
          var nodeId = n.SelectSingleNode("nid").InnerText;
          var body = dBody.SelectSingleNode("//drp_node_revisions[nid='" + nodeId + "']");

          var bodyString = body.SelectSingleNode("body").InnerText;
          if (string.IsNullOrEmpty(bodyString))
            continue; //looks like there are dead nodes

          DateTime created = getTime(n.SelectSingleNode("created"));
          var words = nodeWordDoc.SelectSingleNode("//drp_nodewords[id='" + nodeId + "' and name='keywords']");

          var content = new Content(int.Parse(nodeId))
                          {
                            Title = n.SelectSingleNode("title").InnerText,
                            Created = created,
                            Published = true,
                            MetaKeyWords = words != null ? words.SelectSingleNode("content").InnerText : null
                          };
          content.SetBody(body.SelectSingleNode("body").InnerText);

          var tags = usedTags(nodeId, dTagToContent, tagDoc);
          var comments = getComments(nodeId, commentDoc);
          var attachments = getAttachments(nodeId, attachmentDoc, attachmentRelDoc);

          foreach (var ta in dbTags)
            if (tags.Contains(ta.Name))
              content.AssociateWithTag(ta);
          foreach (var c in comments)
            content.AddComment(c);
          foreach (var a in attachments)
            content.AddAttachment(a);
          session.Save(content);
          Console.WriteLine("Saved {0} with {1} tags, {2} comments and {3} attachments", 
            content.Title, tags.Count, content.CommentCount, content.AttachmentCount);
        }

        tx.Commit();
      }


    }


    private static IEnumerable<Comment> getComments(string nodeID, XmlDocument commentDoc)
    {
      return
        from n in commentDoc.SelectNodes("//drp_comments[nid='" + nodeID + "']").OfType<XmlNode>()

        let email = n.SelectSingleNode("mail").InnerText
        let website = n.SelectSingleNode("homepage").InnerText
        let comment = n.SelectSingleNode("comment").InnerText
        where !string.IsNullOrEmpty(comment)
        select new Comment
                 {
                   Body = n.SelectSingleNode("comment").InnerText,
                   CommenterEmail = string.IsNullOrEmpty(email) ? null : email,
                   CommenterName = n.SelectSingleNode("name").InnerText,
                   CommenterWebsite = string.IsNullOrEmpty(website) ? null : website,
                   Created = getTime(n.SelectSingleNode("timestamp"))
                 };
    }

    private static List<string> usedTags(string nodeId, XmlDocument dTagToContent, XmlDocument tagDoc)
    {
      var tagAssocs =
        from tAssoc in dTagToContent.SelectNodes("//drp_term_node[nid='" + nodeId + "']")
          .OfType<XmlNode>()
        let termId = tAssoc.SelectSingleNode("tid").InnerText
        let tagNode = tagDoc.SelectSingleNode("//drp_term_data[tid='" + termId + "']")
        select tagNode.SelectSingleNode("name").InnerText;

      return tagAssocs.ToList();
    }

    private static IEnumerable<Attachment> getAttachments(string nodeID, XmlDocument attachments, XmlDocument attachmentToNode)
    {
      var atmtIDs = attachmentToNode
        .SelectNodes("//drp_upload[nid='" + nodeID + "']")
        .OfType<XmlNode>()
        .Select(n => n.SelectSingleNode("fid").InnerText);

      if (atmtIDs.Count() == 0) return new Attachment[] {};

      return from n in attachments.SelectNodes("//drp_files[" + listOfAttachments(atmtIDs) + "]").OfType<XmlNode>()
             where n.SelectSingleNode("filename").InnerText != "_original" &&
                   n.SelectSingleNode("filename").InnerText != "thumbnail"
             select new Attachment
             {
               Created = DateTime.Now,
               Name = n.SelectSingleNode("filename").InnerText,
               Path = n.SelectSingleNode("filepath").InnerText,
               Size = Convert.ToInt32(n.SelectSingleNode("filesize").InnerText)
             };
    }

    private static string listOfAttachments(IEnumerable<string> attachmentIDs)
    {
      return string.Join(" or ", attachmentIDs.Select(id => "fid='" + id + "'").ToArray());
    }

    private static XmlDocument getDocument(string doc)
    {
      var document = new XmlDocument();
      document.Load(Path.Combine(DataFolder, doc));
      return document;
    }

    private static void insertTags()
    {
      XmlDocument d = getDocument("drp_term_data.xml");

      using (var session = factory.OpenSession())
      using (var tx = session.BeginTransaction())
      {
        foreach (XmlNode n in d.SelectNodes("//drp_term_data"))
        {
          Tag t = new Tag
                    {
                      Created = DateTime.Now,
                      Description = n.SelectSingleNode("description").InnerText,
                      Name = n.SelectSingleNode("name").InnerText
                    };
          session.Save(t);
          Console.WriteLine("Saved Tag " + t.Name);
        }
        tx.Commit();
        Console.WriteLine("Finished storing tags");
      }
    }

    private static void setupSchema(SessionFactoryMaker maker)
    {
      using (var con = factory.OpenSession().Connection)
      {
        maker.DropAndRecreateSchema(Console.Out, con);
      }
    }

    private static DateTime getTime(XmlNode n)
    {
      return new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(long.Parse(n.InnerText));
    }
  }
}
