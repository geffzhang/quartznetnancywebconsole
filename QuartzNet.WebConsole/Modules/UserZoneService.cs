using System;
using Nancy;
using Nancy.Cookies;
using Nancy.Helpers;

namespace QuartzNet.WebConsole.Modules
{
    public class UserZoneService
    {
        private readonly NancyModule _module;

        public UserZoneService(NancyModule module)
        {
            _module = module;
        }

        public TimeZoneInfo GetUserTimeZone()
        {
            if (!_module.Request.Cookies.ContainsKey("time-zone"))
                return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var timeZoneId = HttpUtility.UrlDecode(_module.Request.Cookies["time-zone"]);
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (Exception)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            }
        }

        public DateTimeOffset? ToUser(DateTimeOffset? dateTimeOffset)
        {
            if (!dateTimeOffset.HasValue)
                return null;
            return ToUser(dateTimeOffset.Value);
        }
        public DateTimeOffset ToUser(DateTimeOffset dateTimeOffset)
        {
            return TimeZoneInfo.ConvertTime(dateTimeOffset, GetUserTimeZone());
        }

        public DateTimeOffset FromUser(DateTime fromUnixTimestamp)
        {
            return TimeZoneInfo.ConvertTimeToUtc(fromUnixTimestamp, GetUserTimeZone());
        }


        public INancyCookie GetCookieFor(string timezoneid)
        {
            return new NancyCookie("time-zone", timezoneid);
        }
    }
}