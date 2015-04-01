﻿using System;
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
            
            //string asp_junk = "__EVENTTARGET=dgHund%24ctl09%24lnkChipnr&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJNDYyMzk0Mzk5DxYGHgpzb3J0Y29sdW1uBQhJRG51bW1lch4Nc29ydGRpcmVjdGlvbgUDQVNDHgNzcWwF0BEgU0VMRUNUICdSJyBBUyB0eXAsIGh1bmRJRCwgcmVnbnIsIGhuYW1uICwgKENBU0UgV0hFTiBIdW5kLklEbnVtbWVyIElOICgnZXhwb3J0JywnaW1wb3J0JykgVEhFTiAnJyBFTFNFIEh1bmQuSURudW1tZXIgRU5EKSBBUyBJRG51bW1lciAsIChDQVNFIFdIRU4gSHVuZC5jaGlwbnIgSU4gKCdleHBvcnQnLCdpbXBvcnQnKSBUSEVOICcnIEVMU0UgSHVuZC5jaGlwbnIgRU5EKSBBUyBjaGlwbnIgLCBoa29uICwgKENBU0UgV0hFTiBSYXMuRkNJbnVtIElTIE5VTEwgVEhFTiAnQmxhbmRyYXMvb3JlZ2lzdHJlcmFkJyBFTFNFIFJhcy5yYXN0ZXh0IEVORCkgQVMgcmFzdGV4dCAsIElTTlVMTCgoU0VMRUNUIERJU1RJTkNUICdqYScgRlJPTSBKQlZfSGlzdG9yaWsgV0hFUkUgSkJWX0hpc3RvcmlrLmh1bmRpZCA9IEh1bmQuaHVuZGlkIEFORCBKQlZfSGlzdG9yaWsuc3RhdHVza29kID0gMzQgICAgQU5EIE5PVCBFWElTVFMgKFNFTEVDVCAqIEZST00gSkJWX0hpc3RvcmlrIEgyIFdIRVJFIEgyLmh1bmRpZCA9IEpCVl9IaXN0b3Jpay5odW5kaWQgQU5EIEgyLnN0YXR1c2tvZCA9IDM1IEFORCAoSDIuaGlzdG9yaWtEYXR1bSA%2BIEpCVl9IaXN0b3Jpay5oaXN0b3Jpa0RhdHVtIE9SIChIMi5oaXN0b3Jpa0RhdHVtID0gSkJWX0hpc3RvcmlrLmhpc3RvcmlrRGF0dW0gQU5EIEgyLmhpc3RvcmlrVGlkID49IEpCVl9IaXN0b3Jpay5oaXN0b3Jpa1RpZCkpKSksJycpIEFTIHNha25hZCBGUk9NIEh1bmQgTEVGVCBPVVRFUiBKT0lOIFJhcyBPTiBSYXMucmFzID0gSHVuZC5yYXMgV0hFUkUgSHVuZC5yYXMgPD4gOTIgQU5EIGNoaXBuciBMSUtFICc3NTIwOTgxMDAzNjU3X18nIEFORCBOT1QgRVhJU1RTICAgIChTRUxFQ1QgKiBGUk9NIEh1bmRzcGFyciwgU3BhcnJUZXh0ICAgICBXSEVSRSBIdW5kc3BhcnIucmVnbnIgPSBIdW5kLnJlZ25yICAgICBBTkQgU3BhcnJUZXh0LnNwYXJyID0gSHVuZHNwYXJyLnNwYXJyICAgICBBTkQgU3BhcnJUZXh0LnZpc2EgPSAwICAgICkgQU5EIE5PVCBFWElTVFMgKFNFTEVDVCAnSHVuZGVuIGF2bGlkZW4nIEZST00gSHVuZHNwYXJyIFdIRVJFIEh1bmRzcGFyci5yZWduciA9IEh1bmQucmVnbnIgQU5EIEh1bmRzcGFyci5zcGFyciA9ICdGJykgVU5JT04gU0VMRUNUICdPJyBBUyB0eXAsIGh1bmRJRCwgJycgQVMgcmVnbnIsIG5hbW4gQVMgaG5hbW4sIElEbnVtbWVyLCBjaGlwbnIsIGtvbiBBUyBoa29uICwgKENBU0UgV0hFTiBSYXMuRkNJbnVtIElTIE5VTEwgVEhFTiAnQmxhbmRyYXMvb3JlZ2lzdHJlcmFkJyBFTFNFIFJhcy5yYXN0ZXh0IEVORCkgQVMgcmFzdGV4dCAsIElTTlVMTCgoU0VMRUNUIERJU1RJTkNUICdqYScgRlJPTSBKQlZfSGlzdG9yaWsgV0hFUkUgSkJWX0hpc3RvcmlrLmh1bmRpZCA9IEpCVl9IdW5kT3JlZy5odW5kaWQgQU5EIEpCVl9IaXN0b3Jpay5zdGF0dXNrb2QgPSAzNCAgICBBTkQgTk9UIEVYSVNUUyAoU0VMRUNUICogRlJPTSBKQlZfSGlzdG9yaWsgSDIgV0hFUkUgSDIuaHVuZGlkID0gSkJWX0hpc3RvcmlrLmh1bmRpZCBBTkQgSDIuc3RhdHVza29kID0gMzUgQU5EIChIMi5oaXN0b3Jpa0RhdHVtID4gSkJWX0hpc3RvcmlrLmhpc3RvcmlrRGF0dW0gT1IgKEgyLmhpc3RvcmlrRGF0dW0gPSBKQlZfSGlzdG9yaWsuaGlzdG9yaWtEYXR1bSBBTkQgSDIuaGlzdG9yaWtUaWQgPj0gSkJWX0hpc3RvcmlrLmhpc3RvcmlrVGlkKSkpKSwnJykgQVMgc2FrbmFkIEZST00gSkJWX0h1bmRPcmVnIExFRlQgT1VURVIgSk9JTiBSYXMgT04gUmFzLnJhcyA9IEpCVl9IdW5kT3JlZy5yYXNrb2QgV0hFUkUgMT0xIEFORCBjaGlwbnIgTElLRSAnNzUyMDk4MTAwMzY1N19fJyBBTkQgTk9UIEVYSVNUUyAoU0VMRUNUICogRlJPTSBIdW5kIFdIRVJFIEh1bmQucmFzIDw%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%2BPGJyPkzDpHMgbWVyIG9tIDxhIHN0eWxlPSdGT05ULVdFSUdIVDogbm9ybWFsOyBDT0xPUjogIzMzMzNjYzsgVEVYVC1ERUNPUkFUSU9OOiB1bmRlcmxpbmUnIGhyZWY9amF2YXNjcmlwdDpJRG1hcmtuaW5nKCk7PmlkLW3DpHJrbmluZzwvYT4gYXYgaHVuZCwgdGlwcyBww6UgaHVyIGR1IGzDpHNlciBhdiBlbiBpZC1tw6Rya25pbmcgbS5tLh8EaGRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYCBQpjaGtTYWtuYWRlBQlhZ3JpYV9zb2umgVkJoOM5AJr3k3qJ5%2FQrNisWTKr89Pb49Fip4xW7lg%3D%3D&__EVENTVALIDATION=%2FwEdADb3rbZUNtv04e6Acg%2BTp6wzQig0%2BhS%2FUdd5HPXCFNQ%2BrEDpxSMGnoDsU0cnCByiMqLUL%2FtJfXv%2F9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V%2FOY%2FMBYAJRFoITXGBm%2BL%2B8iOMRuGYLd8AR00eviexQRBuO5ukmYfkkrHAMizX8BpLg6R55LNGaGloKckQWSqgyaCy7kBgtk%2FhBccfKsA0LtCpIvqhKLKxGIFaMwZL4wJPeshBM8sA9tIH7B%2Fj8bRRsO4r7%2FsWgdDNl5WxflIA05Fn60TO7HvAeAJeV41Gke4DBlJuaXwpenqtbFPCtEpSYwTMv4owgNuQHwCrY2c6yWh0W8B7NGf2UDL%2FGFIeN80qca0B%2BWWzcvY3cAffBZyi4iX7BzF8L1vE2RzHRG3YP%2BI5bu6nL6uOzbcj%2FL%2FQCgvyqdGzdpuhebQIqqUjTwFDFITix4gRvhWZhJx7XRxcxBVSPTmu%2BAVkwka8WpyAstHRbnzEXEzct%2FtwISTbBGEdq3C9cK%2FuP8FTZHrWpPVF8Ewb5cM1TmEp5chjqIcYglE%2BR6zIXoPK2OVA8gYO4DS7LpXuIdKbN7fpPIJyK3bN%2Bz6zZDGGqfPU8rxFony8jhhmGuu3n%2Fq9n%2Fq8KXkd%2Ba0z%2B2iSepl%2FcsSC9fgIdWHcR7%2F2PFxwM60ElP9eF4vCJ9erGBqiniJ2NcGPJCPbox%2BOiYIo8GHbCyM47zHev9GL%2BxuwBayEPwx2s0p5VtYQu%2FjOrsxXkmF1ZpYOMXOkbScKsEGEhVYKCyewsJJ%2FCEgMkeWxStjgjQWHfmgs3iENgaTPwRHYRmEIp5s6gig9Yggi7Ygv395OvJLudd5NHeF1HtSgvCf%2F2c8681iXrbF96lzU7p0%2FF27PtxnPiUhmBsSanWhCn1jKMN%2BMmA50Wx6%2B4WRHd9p25MFN4qmuyelF%2BrEgVveKtk5F0KqzyjxdS3VbCkZZqMoLTFappxLcf7mVuNP3Ihu9kXdwxfHz%2BxpO%2BQZgTpoxRsVxtvbHFODxT%2FoprSDKlUbEhIi203AACntPmY2gVmo4iB3jJ0YUfZgkZtwjVFJhdDvmsJpJUjr%2BIrr6PbetHOjX0K4gTCFK2bsdlLziDc%2BwRikDUSkylMpZSVHYiYYRVqDARaQMAVWv6fWE5Ez2YWi4e0FpkLYQP7J%2FfsAfUAKjdD8WDFv%2BsDF0fPnUmkw%3D%3D&";
            string asp_junk = "__EVENTTARGET=btnSearch&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKMTQ2MzQ3MTA0Mw9kFgICAQ9kFgoCBQ8PZBYCHgVzdHlsZQUMd2lkdGg6MTE1cHg7ZAIJDw9kFgIfAAUMd2lkdGg6MTE1cHg7ZAIRDw8WAh4HVmlzaWJsZWhkZAITDzwrAAsAZAIVDw8WAh4EVGV4dAW9AUzDpHMgbWVyIG9tIDxhIHN0eWxlPSdGT05ULVdFSUdIVDogbm9ybWFsOyBDT0xPUjogIzMzMzNjYzsgVEVYVC1ERUNPUkFUSU9OOiB1bmRlcmxpbmUnIGhyZWY9amF2YXNjcmlwdDpJRG1hcmtuaW5nKCk7PmlkLW3DpHJrbmluZzwvYT4gYXYgaHVuZCwgdGlwcyBww6UgaHVyIGR1IGzDpHNlciBhdiBlbiBpZC1tw6Rya25pbmcgbS5tLmRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYEBQpjaGtTYWtuYWRlBQlhZ3JpYV9zb2sFEmZiX0h1bmRhcl9tYXJnaW5hbAUKcGV0bmV0X3Nva%2FjfvFKh4WmTdvZRcBFxIAHVKBRZE7dGUZVk%2FTOCoYU4&__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=%2FwEdAAjXcifg0%2BnqW%2Bfm4DqM2aqqQig0%2BhS%2FUdd5HPXCFNQ%2BrEDpxSMGnoDsU0cnCByiMqLUL%2FtJfXv%2F9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V%2FOY4RVqDARaQMAVWv6fWE5Ez17OpEVAnRAz%2Bni8Ag5n2QpfOMoOXtqZvIKswGfKT6%2Bs0JH2fO6i9sZdPdiI%2F%2F%2BvAn4uDNuBx79a3zCQR1TOjfs&";
            WebRequest req = WebRequest.Create("http://hundar.skk.se/agarreg/hund_sok.aspx");

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
            WebRequest req = WebRequest.Create("http://hundar.skk.se/agarreg/Hund.aspx?hundID=" + animal.DbId);

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
            //string asp_junk = "__EVENTTARGET=btnSearch&__LASTFOCUS=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJMTE0MDUyODE5DxYGHgpzb3J0Y29sdW1uBQhJRG51bW1lch4Nc29ydGRpcmVjdGlvbgUDQVNDHgNzcWwFkwcgU0VMRUNUIGthdHRJRCwgSVNOVUxMKElEbnVtbWVyLCcnKSBBUyBJRG51bW1lciwgSVNOVUxMKGNoaXBudW1tZXIsJycpIEFTIGNoaXBudW1tZXIsIEthdHQuc3BhcnJrb2QgLCBJU05VTEwoa2F0dG5hbW4sJycpIEFTIGthdHRuYW1uLCBmb2RlbHNlYXIgLCBJU05VTEwocmFzdGV4dCwnJykgQVMgcmFzdGV4dCwgSVNOVUxMKGZhcmcsJycpIEFTIGZhcmcgLCAoQ0FTRSBrb24gV0hFTiAnSCcgVEhFTiAnSGFuZScgV0hFTiAnVCcgVEhFTiAnSG9uYScgRUxTRSAnJyBFTkQpIEFTIGtvbiAsIChTRUxFQ1QgVE9QIDEgS1Aub3J0IEZST00gS2F0dFBlcnNvbiBLUCwgS2F0dEFlZ2FyZSBLQSAgICBXSEVSRSBLQS5wZXJzb25JRCA9IEtQLnBlcnNvbklEIEFORCBLQS5rYXR0SUQgPSBLYXR0LmthdHRJRCkgQVMgb3J0ICwgSVNOVUxMKChTRUxFQ1QgRElTVElOQ1QgJ2phJyBGUk9NIEthdHRIaXN0b3JpayBXSEVSRSBLYXR0SGlzdG9yaWsua2F0dGlkID0gS2F0dC5rYXR0aWQgQU5EIEthdHRIaXN0b3Jpay5zdGF0dXNrb2QgPSAzNCAgICBBTkQgTk9UIEVYSVNUUyAoU0VMRUNUICogRlJPTSBLYXR0SGlzdG9yaWsgS0gyIFdIRVJFIEtIMi5rYXR0aWQgPSBLYXR0SGlzdG9yaWsua2F0dGlkIEFORCBLSDIuc3RhdHVza29kID0gMzUgQU5EIChLSDIuaGlzdG9yaWtEYXR1bSA%2BIEthdHRIaXN0b3Jpay5oaXN0b3Jpa0RhdHVtIE9SIChLSDIuaGlzdG9yaWtEYXR1bSA9IEthdHRIaXN0b3Jpay5oaXN0b3Jpa0RhdHVtIEFORCBLSDIuaGlzdG9yaWtUaWQgPj0gS2F0dEhpc3RvcmlrLmhpc3RvcmlrVGlkKSkpKSwnJykgQVMgc2FrbmFkIEZST00gS2F0dCBXSEVSRSAxPTEgQU5EIGNoaXBudW1tZXIgTElLRSAnNzUyMDk4MTAwMzY1N19fJyBBTkQgKEthdHQuc3BhcnJrb2QgSVMgTlVMTCBPUiBLYXR0LnNwYXJya29kIDw%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%2BIGF2IGthdHQsIHRpcHMgcMOlIGh1ciBkdSBsw6RzZXIgYXYgZW4gaWQtbcOkcmtuaW5nIG0ubS4fBGhkZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAgUKY2hrU2FrbmFkZQUJYWdyaWFfc29rAgmkY%2BPZp2HEbloNew1cGc2X%2FhLWGNDMaGGbmcQaewg%3D&__EVENTVALIDATION=%2FwEdAC0uLoR5MoI0UBRhU7rrmhf9Qig0%2BhS%2FUdd5HPXCFNQ%2BrEDpxSMGnoDsU0cnCByiMqLUL%2FtJfXv%2F9aZTYdTCcZiSjtTdVzRZn7DFyWrI8V%2FOY7YmfCcCMWJbwZPhMxiT1P42GE9BKkvCBfXIlABDFOkHoAcmp%2Bqq7aiXQIs3AUJ%2FCZYyPtV03Zaoma00hQNXNVtxgHKHJmIyGGNsoIDnBmUF36Byyx6uF%2FkMQ%2BRS%2FzxQR9JPh7rTHznn3wC79Yuc5tO9BDvEYfd8RBAVU%2FtgQj6GI8XNHboQuq6VmLwgYN3IYZSSBhZGwecYh7wT%2B2qzr4RYsm2YKcJp3AHKLJPgb1NPR6CYXUXd74cEibpAbgWJcCp%2B6%2Fd9Pe5dY3fDc3uZCJ3iWgW9RHruV52X5%2F8i5n2s1UnwntjtQHJZJlq81SgxFtD2yJARBbwed8Iz2sd689RAf56Drpbb98xbQtCuN%2BZcyeulQ2%2Bfhsgzdcwsm%2FDwWiiiS2iirYAfDocCsf78sO4XUGKYY8sICmUT21skCHxxIIN5U1AFT07XxLLU1gAzU1S4JZWn1Ooin2JvI0L9StWAYJKZ5otdIfsXcf7%2BTZ1uhmVk2%2FzCiFAlls0BPrPeAft87yt76I1%2B4gajobDoBoM68%2FUZQ%2BVFucKcpDgwNf2%2Bct69BRzlVQc1HjBc4UTUTerQwaH1CXmaOEbgPmgqbI35fuFP5lwofw%2FCUwpgSECtN6wQ%2BY0C4ZI%2FPDiI4ynV6fO9aFIvqGwbEpGeqyyXH7uk9AwdrHyXTmS9Ct8eFBvQ4kbOnoIoUPuj2pHpZ%2Fyr4e8g1bjiYrStr%2F%2B1Goj9kuYmeotTEy0waIUqx2%2BpAT4u3j0jbMRbYM5uMknpDS1il5%2BS63yHmGSY6eJ7%2FevBiw8J3GlAUBZspNhQnaPfgi7%2FC%2Bj91e7G7j2UuxYLBsxy%2F4RVqDARaQMAVWv6fWE5Ez38gl3uhhvLrWs7Ohl4kmBB%2F%2BrofJN3GEVzk2ZX7tXACw%3D%3D&";

            WebRequest req = WebRequest.Create("http://hundar.skk.se/agarreg/katt_sok.aspx");

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
            WebRequest req = WebRequest.Create("http://hundar.skk.se/agarreg/Katt.aspx?kattID=" + animal.DbId);

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";

            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }
    }
}
