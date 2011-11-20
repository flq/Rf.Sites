﻿using System.Linq;

namespace Rf.Sites.Features.Models
{
    public class ContentTeaserPage : Page<ContentTeaserVM>
    {
        public ContentTeaserPage(IPagingArgs paging, IQueryable<ContentTeaserVM> query) : base(paging, query)
        {
        }
    }

    public class PersonalEntryPage : Page<ContentTeaserVM>
    {
        public PersonalEntryPage(IQueryable<ContentTeaserVM> query)
            : base(new PagingArgs { Page = 0 }, query)
        {
        }
    }
}