using System;
using System.Xml;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SKKSearchAPI;

namespace SKKRegisterSok
{

    public class SKKSearch : SKKSearchAPI.SKKSearch
    {
        private Requests _req = new Requests();

        private static String VIEW_STATE = "__VIEWSTATE";

        /// <summary>
        /// Search dogs
        /// </summary>
        /// <param name="idMode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AnimalList SearchDogs(IdModell idMode, String id)
        {
            string chipId = idMode == IdModell.Chip ? id : "";
            string inkId = idMode == IdModell.Tatuering ? id : "";

            String response = _req.DoDogRequest(inkId, chipId);

            if (response != String.Empty)
            {
                return ParseDogs(response, inkId, id);
            }
            throw new Exception("Empty response");
        }

        /// <summary>
        /// Parse dog search response HTML
        /// </summary>
        /// <param name="response"></param>
        /// <param name="inkId"></param>
        /// <param name="chipId"></param>
        /// <returns></returns>
        private AnimalList ParseDogs(String response, String inkId, String chipId)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var dogTableNode = doc.DocumentNode.SelectNodes("//table[@id='dgHund']");

            // If we got some dogs, save the view state
            if (dogTableNode != null)
            {
                ViewStateParser parser = new ViewStateParser();
                var list = parser.ParseViewStateGraph(getInputValue(doc, VIEW_STATE), Djurslag.Hund);
                
                var next20 = doc.DocumentNode.SelectNodes("//a[@href='javascript:__doPostBack(&#39;dgHund$ctl24$ctl01&#39;,&#39;&#39;)']");
                if (next20 != null)
                    list.HasMoreThan20 = true;

                return list;
            }

            return new AnimalList { errorMessage = ErrorMessages.NoDogsFound };
        }

        /// <summary>
        /// Search for a specific dog
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        public Animal GetDogDetails(Animal dog) {

            // Make the request
            String response = _req.DoSpecDogRequest(dog);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            // TODO validate that we have a relevant page
            if (doc.DocumentNode != null)
            {
                return ParseDog(doc, dog);
            }

            throw new Exception("Ett fel uppstod när inläsning av hund genomfördes.");
        }

        /// <summary>
        /// Parse specific dog response
        /// </summary>
        /// <param name="catTableNodes"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        private Animal ParseDog(HtmlDocument dogTableNodes, Animal dog)
        {
            var chipNumberNode = dogTableNodes.DocumentNode.SelectNodes("//span[@id='lblChipnr']");

            if (chipNumberNode == null)
            {
                throw new Exception("Hittade inte hunden.");
            }

            if (chipNumberNode[0].InnerText.Trim() != dog.ChipId)
            {
                throw new Exception("Fel hund hittades.");
            }

            dog.Harlag = getValue(dogTableNodes, "lblHarlag");
            dog.Fodelsedatum = getValue(dogTableNodes, "lblFodelsedatum");
            dog.Kon = getValue(dogTableNodes, "lblKon") == "T" ? Kon.Tik : Kon.Hund;
            dog.Farg = getValue(dogTableNodes, "lblFarg");

            var agareTableNodes = dogTableNodes.DocumentNode.SelectNodes("//table[@id='ctl00_tblAgare']");

            if (agareTableNodes != null && agareTableNodes[0] != null)
            {
                var agareTableNode = agareTableNodes[0];

                var agareRows = agareTableNode.SelectNodes("tr");
                if (agareRows != null)
                {
                    // First row
                    dog.Agare.Namn = agareRows[0].ChildNodes[2].InnerText;
                    dog.Agare.TelHem = agareRows[0].ChildNodes[5].InnerText;
                    // Second row
                    dog.Agare.Adress = agareRows[1].ChildNodes[2].InnerText;
                    dog.Agare.TelArbete = agareRows[1].ChildNodes[5].InnerText;
                    // Third row
                    dog.Agare.Adress += ", " + agareRows[2].ChildNodes[2].InnerText.Replace("&nbps", "");
                    dog.Agare.TelMobil = agareRows[2].ChildNodes[5].InnerText;
                    // Fourth row
                    dog.Agare.Epost += agareRows[3].ChildNodes[2].InnerText;
                }
            }

            dog.TrimStrings();

            return dog;
        }

        /// <summary>
        /// Search cats
        /// </summary>
        /// <param name="idMode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AnimalList SearchCats(IdModell idMode, String id)
        {
            string chipId = idMode == IdModell.Chip ? id : "";
            string inkId = idMode == IdModell.Tatuering ? id : "";

            String response = _req.DoCatRequest(inkId, chipId);

            if (response != String.Empty)
            {
                return ParseCats(response, inkId, chipId);
            }
            throw new Exception("Empty response");
        }

        /// <summary>
        /// Parse cat search response HTML
        /// </summary>
        /// <param name="response"></param>
        /// <param name="inkId"></param>
        /// <param name="chipId"></param>
        /// <returns></returns>
        private AnimalList ParseCats(String response, String inkId, String chipId)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var catTableNode = doc.DocumentNode.SelectNodes("//table[@id='dgKatt']");

            // If we got some dogs, save the view state
            if (catTableNode != null)
            {
                ViewStateParser parser = new ViewStateParser();
                var list = parser.ParseViewStateGraph(getInputValue(doc, VIEW_STATE), Djurslag.Katt);

                var next20 = doc.DocumentNode.SelectNodes("//a[@href='javascript:__doPostBack(&#39;dgKatt$ctl24$ctl01&#39;,&#39;&#39;)']");
                if (next20 != null)
                    list.HasMoreThan20 = true;

                return list;
            }

            return new AnimalList { errorMessage = "No cats found" };
        }

        /// <summary>
        /// Search for a specific cat
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public Animal GetCatDetails(Animal cat)
        {
            // Make the request
            String response = _req.DoSpecCatRequest(cat);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            // TODO validate that we have a relevant page
            if (doc.DocumentNode != null)
            {
                return ParseCat(doc, cat);
            }

            throw new Exception("Ett fel uppstod när inläsning av katt genomfördes.");
        }

        /// <summary>
        /// Parse specific cat response
        /// </summary>
        /// <param name="catTableNodes"></param>
        /// <param name="dog"></param>
        /// <returns></returns>
        private Animal ParseCat(HtmlDocument catTableNodes, Animal cat)
        {
            var chipNumberNode = catTableNodes.DocumentNode.SelectNodes("//span[@id='lblChipnr']");

            if (chipNumberNode == null)
            {
                throw new Exception("Hittade inte katten.");
            }

            if (chipNumberNode[0].InnerText.Trim() != cat.ChipId)
            {
                throw new Exception("Fel katt hittades.");
            }

            cat.Harlag = getValue(catTableNodes, "lblHarlag");

            cat.Farg = getValue(catTableNodes, "lblFarg");

            var agareTableNodes = catTableNodes.DocumentNode.SelectNodes("//table[@id='ctl00_tblAgare']");

            if (agareTableNodes != null && agareTableNodes[0] != null)
            {
                var agareTableNode = agareTableNodes[0];

                var agareRows = agareTableNode.SelectNodes("tr");
                if (agareRows != null && agareRows.Count > 1)
                {
                    // First row
                    cat.Agare.Namn = agareRows[0].ChildNodes[2].InnerText;
                    cat.Agare.TelHem = agareRows[0].ChildNodes[5].InnerText;
                    // Second row
                    cat.Agare.Adress = agareRows[1].ChildNodes[2].InnerText;
                    cat.Agare.TelArbete = agareRows[1].ChildNodes[5].InnerText;
                    // Third row
                    cat.Agare.Adress += ", " + agareRows[2].ChildNodes[2].InnerText.Replace("&nbps", "");
                    cat.Agare.TelMobil = agareRows[2].ChildNodes[5].InnerText;
                }
                else
                {
                    cat.Agare.Namn = "Ingen ägarinformation";
                }
            }

            return cat;
        }

        /// <summary>
        /// Find the id inside a span
        /// </summary>
        /// <param name="dogTableNodes"></param>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private String getValue(HtmlDocument dogTableNodes, String idToFind) {
            var node = dogTableNodes.DocumentNode.SelectNodes("//span[@id='" + idToFind + "']");
            if (node != null && node[0] != null)
            {
                return node[0].InnerText;
            }
            return "";
        }

        /// <summary>
        /// Get value of an input field
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private String getInputValue(HtmlDocument doc, String idToFind)
        {
            var metaNode = doc.DocumentNode.SelectNodes("//input[@id='" + idToFind + "']");
            if (metaNode != null && metaNode[0].Attributes.Count > 0)
            {
                var attribute = metaNode[0].Attributes.First(a => a.Name == "value");
                if (attribute != null && attribute.Value != null)
                {
                    return attribute.Value;
                }
            }
            return null;
        }
    }
}