using System;

namespace QuartzNet.WebConsole.AccessControl
{
    public class AclRule
    {
        private readonly Func<string, bool?> _user;
        private readonly Func<string, bool?> _action;
        private readonly Func<string, bool?> _job;

        public AclRule(Func<string, bool?> user, Func<string, bool?> action, Func<string, bool?> job)
        {
            _user = user;
            _action = action;
            _job = job;
        }

        public bool? Verify(string user, string action, string job)
        {
            return _user(user) & _action(action) & _job(job);
        }
    }
}