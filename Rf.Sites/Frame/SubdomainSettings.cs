using System;
using System.Collections.Generic;

namespace Rf.Sites.Frame
{
    public class SubdomainSettings
    {
        private readonly Dictionary<string,string> _sites = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                string result;
                return _sites.TryGetValue(key, out result) ? result : null;
            }
        }

        public string ModelQualifiedName { get; set; }

        public string Site1
        {
            set
            {
                Add(value);
            }
        }

        private void Add(string value)
        {
            var things = value.Split('=');
            if (things.Length != 2) return;
            var keys = things[0].Split(';');
            foreach (var key in keys)
                _sites.Add(key, things[1]);
        }

        public object GetHomeInputModel(string serverName)
        {
            var model = this[serverName];
            if (model == null)
                return null;
            var typeName = string.Format(ModelQualifiedName, model);
            try
            {
                var t = Type.GetType(typeName);
                return Activator.CreateInstance(t);
            }
            catch (Exception x)
            {
                return null;
            }
        }
    }
}