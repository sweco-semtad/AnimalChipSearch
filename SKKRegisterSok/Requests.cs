using System;
using System.IO;
using System.Net;
using SKKSearchAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKRegisterSok
{
    public class Requests
    {

        /// <summary>
        /// Request for dog search
        /// </summary>
        /// <param name="tatooId"></param>
        /// <param name="chiId"></param>
        /// <returns></returns>
        public String DoDogRequest(String tatooId, String chiId) {
            
            string asp_junk = "__EVENTTARGET=btnSearch&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKMTQ2MzQ3MTA0Mw9kFgICAQ9kFgoCBQ8PZBYCHgVzdHlsZQUMd2lkdGg6MTE1cHg7ZAIJDw9kFgIfAAUMd2lkdGg6MTE1cHg7ZAIRDw8WAh4HVmlzaWJsZWhkZAITDzwrAAsAZAIVDw8WAh4EVGV4dAW9AUzDpHMgbWVyIG9tIDxhIHN0eWxlPSdGT05ULVdFSUdIVDogbm9ybWFsOyBDT0xPUjogIzMzMzNjYzsgVEVYVC1ERUNPUkFUSU9OOiB1bmRlcmxpbmUnIGhyZWY9amF2YXNjcmlwdDpJRG1hcmtuaW5nKCk7PmlkLW3DpHJrbmluZzwvYT4gYXYgaHVuZCwgdGlwcyBww6UgaHVyIGR1IGzDpHNlciBhdiBlbiBpZC1tw6Rya25pbmcgbS5tLmRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYEBQpjaGtTYWtuYWRlBQlhZ3JpYV9zb2sFEmZiX0h1bmRhcl9tYXJnaW5hbAUKcGV0bmV0X3Nva%2FjfvFKh4WmTdvZRcBFxIAHVKBRZE7dGUZVk%2FTOCoYU4&__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=%2FwEdAAjXcifg0%2BnqW%2Bfm4DqM2aqqQig0%2BhS%2FUdd5HPXCFNQ%2BrEDpxSMGnoDsU0cnCByiMqLUL%2FtJfXv%2F9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V%2FOY4RVqDARaQMAVWv6fWE5Ez17OpEVAnRAz%2Bni8Ag5n2QpfOMoOXtqZvIKswGfKT6%2Bs0JH2fO6i9sZdPdiI%2F%2F%2BvAn4uDNuBx79a3zCQR1TOjfs&";
            WebRequest req = WebRequest.Create(SKKUrls.DogSearchUrl);

            string searchString = "txtIDnummer=" + tatooId + "&txtChipNr=" + chiId + "&btnSearch=S%C3%B6k";

            byte[] send = Encoding.Default.GetBytes(asp_junk + searchString);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = send.Length;

            Stream sout = req.GetRequestStream();
            sout.Write(send, 0, send.Length);
            sout.Flush();
            sout.Close();

            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Request to get information on a specific dog
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public String DoSpecDogRequest(Animal animal)
        {
            WebRequest req = WebRequest.Create(SKKUrls.DogUrl + animal.DbId);

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Request for cat search
        /// </summary>
        /// <param name="tatooId"></param>
        /// <param name="chiId"></param>
        /// <returns></returns>
        public String DoCatRequest(String tatooId, String chiId)
        {
            string asp_junk = "__EVENTTARGET=btnSearch&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKLTY5ODQwNjM4NQ9kFgICAQ9kFgoCBQ8PZBYCHgVzdHlsZQULd2lkdGg6NjhweDtkAgkPD2QWAh8ABQx3aWR0aDoxMTlweDtkAhEPDxYCHgdWaXNpYmxlaGRkAhMPPCsACwBkAhUPDxYCHgRUZXh0Bb0BTMOkcyBtZXIgb20gPGEgc3R5bGU9J0ZPTlQtV0VJR0hUOiBub3JtYWw7IENPTE9SOiAjMzMzM2NjOyBURVhULURFQ09SQVRJT046IHVuZGVybGluZScgaHJlZj1qYXZhc2NyaXB0OklEbWFya25pbmcoKTs%2BaWQtbcOkcmtuaW5nPC9hPiBhdiBrYXR0LCB0aXBzIHDDpSBodXIgZHUgbMOkc2VyIGF2IGVuIGlkLW3DpHJrbmluZyBtLm0uZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgQFCmNoa1Nha25hZGUFCWFncmlhX3NvawUSZmJfS2F0dGVyX21hcmdpbmFsBQpwZXRuZXRfc29rK6RW00%2BITnZAeWrdOx2RzJnkVnWiv4ByAvZieLhca70%3D&__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=%2FwEdAAhAo058sOaTLafkoh5hIRB4Qig0%2BhS%2FUdd5HPXCFNQ%2BrEDpxSMGnoDsU0cnCByiMqLUL%2FtJfXv%2F9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V%2FOY4RVqDARaQMAVWv6fWE5Ez2yRkirKlw8T%2BEfZzLqxo%2ByfOMoOXtqZvIKswGfKT6%2Bs1zzkHPxTHraeF%2BGFfN71Z%2BoRyYASU3h5eqHeUbnIeRG&";

            WebRequest req = WebRequest.Create(SKKUrls.CatSearchUrl);

            string searchString = "txtIDnummer=" + tatooId + "&txtChipNr=" + chiId + "&btnSearch=S%C3%B6k";

            byte[] send = Encoding.Default.GetBytes(asp_junk + searchString);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = send.Length;

            Stream sout = req.GetRequestStream();
            sout.Write(send, 0, send.Length);
            sout.Flush();
            sout.Close();

            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Request for specific cat information
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public String DoSpecCatRequest(Animal animal)
        {
            WebRequest req = WebRequest.Create(SKKUrls.CatUrl + animal.DbId);

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }
    }
}
