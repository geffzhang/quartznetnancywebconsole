using System;
using System.Collections.Generic;
using System.Linq;

namespace QuartzNet.WebConsole.AccessControl
{
    public class AclListBuilder
    {
        private readonly List<AclRule> _rules = new List<AclRule>();
        public void Add(Func<string, bool?> user = null, Func<string, bool?> action = null,
                        Func<string, bool?> job = null)
        {
            user = user ?? (s => true);
            action = action ?? (s => true);
            job = job ?? (s => true);

            _rules.Add(new AclRule(user, action, job));
        }

        public bool Verify(string user, string action, string job)
        {
            var result = _rules.Select(r => r.Verify(user, action, job)).FirstOrDefault(r => r.HasValue);
            return result ?? false;
        }
    }
}