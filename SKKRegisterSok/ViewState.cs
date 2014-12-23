using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKRegisterSok
{
    public class ViewState
    {
        public static String EVENT_TARGET = "__EVENTTARGET";
        public static String LAST_FOCUS = "__LASTFOCUS";
        public static String EVENT_ARGUMENT = "__EVENTARGUMENT";
        public static String VIEW_STATE = "__VIEWSTATE";
        public static String TATOO_ID = "txtIDnummer";
        public static String CHIP_ID = "txtChipNr";
        public static String EVENT_VALIDATION = "__EVENTVALIDATION";

        private static String AND = "&";
        private static String ASSIGN = "=";

        public String EventTarget { get; set; }
        public String LastFocus { get; set; }
        public String EventArgument { get; set; }
        public String ViewStateString { get; set; }
        public String TatooId { get; set; }
        public String ChipId { get; set; }

        public static String EventValidationString = "/wEdADadg7OaBWZtkc4R5iEiT6WCQig0+hS/Udd5HPXCFNQ+rEDpxSMGnoDsU0cnCByiMqLUL/tJfXv/9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V/OY/MBYAJRFoITXGBm+L+8iOMRuGYLd8AR00eviexQRBuO5ukmYfkkrHAMizX8BpLg6R55LNGaGloKckQWSqgyaCy7kBgtk/hBccfKsA0LtCpIvqhKLKxGIFaMwZL4wJPeshBM8sA9tIH7B/j8bRRsO4r7/sWgdDNl5WxflIA05Fn60TO7HvAeAJeV41Gke4DBlJuaXwpenqtbFPCtEpSYwTMv4owgNuQHwCrY2c6yWh0W8B7NGf2UDL/GFIeN80qca0B+WWzcvY3cAffBZyi4iX7BzF8L1vE2RzHRG3YP+I5bu6nL6uOzbcj/L/QCgvyqdGzdpuhebQIqqUjTwFDFITix4gRvhWZhJx7XRxcxBVSPTmu+AVkwka8WpyAstHRbnzEXEzct/twISTbBGEdq3C9cK/uP8FTZHrWpPVF8Ewb5cM1TmEp5chjqIcYglE+R6zIXoPK2OVA8gYO4DS7LpXuIdKbN7fpPIJyK3bN+z6zZDGGqfPU8rxFony8jhhmGuu3n/q9n/q8KXkd+a0z+2iSepl/csSC9fgIdWHcR7/2PFxwM60ElP9eF4vCJ9erGBqiniJ2NcGPJCPbox+OiYIo8GHbCyM47zHev9GL+xuwBayEPwx2s0p5VtYQu/jOrsxXkmF1ZpYOMXOkbScKsEGEhVYKCyewsJJ/CEgMkeWxStjgjQWHfmgs3iENgaTPwRHYRmEIp5s6gig9Yggi7Ygv395OvJLudd5NHeF1HtSgvCf/2c8681iXrbF96lzU7p0/F27PtxnPiUhmBsSanWhCn1jKMN+MmA50Wx6+4WRHd9p25MFN4qmuyelF+rEgVveKtk5F0KqzyjxdS3VbCkZZqMoLTFappxLcf7mVuNP3Ihu9kXdwxfHz+xpO+QZgTpoxRsVxtvbHFODxT/oprSDKlUbEhIi203AACntPmY2gVmo4iB3jJ0YUfZgkZtwjVFJhdDvmsJpJUjr+Irr6PbetHOjX0K4gTCFK2bsdlLziDc+wRikDUSkylMpZSVHYiYYRVqDARaQMAVWv6fWE5Ez3pdegB5X9EAOV9yakr5CjjKOw7Cf4QtudKWXlmhPmODQ==";

        public override String ToString()
        {
            var strb = new StringBuilder();
            strb.Append(EVENT_TARGET);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(EventTarget));
            //strb.Append(EventTarget);

            strb.Append(AND);
            strb.Append(LAST_FOCUS);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(LastFocus));
            //strb.Append(LastFocus);

            strb.Append(AND);
            strb.Append(EVENT_ARGUMENT);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(EventArgument));
            //strb.Append(EventArgument);

            strb.Append(AND);
            strb.Append(VIEW_STATE);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(ViewStateString));
            //strb.Append(ViewStateString);

            strb.Append(AND);
            strb.Append(EVENT_VALIDATION);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(EventValidationString));
            //strb.Append(EventValidationString);

            strb.Append(AND);
            strb.Append(TATOO_ID);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(TatooId));

            strb.Append(AND);
            strb.Append(CHIP_ID);
            strb.Append(ASSIGN);
            strb.Append(WebUtility.UrlEncode(ChipId));

            return strb.ToString();
        }
    }
}
