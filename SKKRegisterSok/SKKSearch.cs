using System;
using System.Xml;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SKKRegisterSok
{
    public class SKKSearch
    {
        private Requests _req = new Requests();

        public AnimalList SearchDogs(String inkId, String chiId)
        {
            String response = _req.DoDogRequest(inkId, chiId);

            if (response != String.Empty)
            {
                return Parse(response, inkId, chiId);
            }
            throw new Exception("Empty response");
        }

        private AnimalList Parse(String response, String inkId, String chipId)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var dogTableNode = doc.DocumentNode.SelectNodes("//table[@id='dgHund']");

            // If we got some dogs, save the view state
            if (dogTableNode != null)
            {
                //var animalList = ParseDogs(dogTableNode);

                //if (animalList.animals.Count > 0)
                //{
                //    var viewState = animalList.viewState;
                //    viewState.EventTarget = getInputValue(doc, ViewState.EVENT_TARGET);
                //    viewState.EventArgument = getInputValue(doc, ViewState.EVENT_ARGUMENT);
                //    viewState.LastFocus = getInputValue(doc, ViewState.LAST_FOCUS);
                //    viewState.ViewStateString = getInputValue(doc, ViewState.VIEW_STATE);
                //    viewState.EventTarget = getInputValue(doc, ViewState.EVENT_TARGET);
                //    viewState.TatooId = inkId;
                //    viewState.ChipId = chipId;

                //    // First, deserialize the string into a Triplet
                //    LosFormatter los = new LosFormatter();
                //    object viewStateObj = los.Deserialize(viewState.ViewStateString);

                //    byte[] data = Convert.FromBase64String(viewState.ViewStateString);
                //    string decodedString = Encoding.UTF8.GetString(data);
                //}

                ViewStateParser parser = new ViewStateParser();
                return parser.ParseViewStateGraph(getInputValue(doc, ViewState.VIEW_STATE));
            }

            var list = new AnimalList();
            list.errorMessage = "No dogs found";
            return list;
        }

        //private AnimalList ParseDogs(HtmlNodeCollection dogTableNodes)
        //{
        //    AnimalList resultList = new AnimalList();

        //    HtmlNodeCollection dogRows = dogTableNodes[0].SelectNodes("//tr[@class='datagridItem']");

        //    foreach (HtmlNode dogRow in dogRows)
        //    {
        //        var animal = new Animal();

        //        int columnIndex = 0;
        //        foreach (HtmlNode tdNode in dogRow.ChildNodes)
        //        {
        //            if (tdNode.Name != "td")
        //            {
        //                continue;
        //            }

        //            switch (columnIndex)
        //            {
        //                case 0:
        //                    var nodes = tdNode.SelectNodes("font/a");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode inkAnchorNode = nodes[0];
        //                        animal.TatueringsId = inkAnchorNode.InnerText;
        //                    }
        //                    break;
        //                case 1:
        //                    nodes = tdNode.SelectNodes("font/a");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode chipAnchorNode = nodes[0];
        //                        animal.ChipId = chipAnchorNode.InnerText;
        //                        var jsLink = chipAnchorNode.Attributes[1].Value;
        //                        animal.linkNum = jsLink.Replace("javascript:__doPostBack(&#39;", "").Replace("&#39;,&#39;&#39;)", "");
        //                    }
        //                    break;
        //                case 2:
        //                    nodes = tdNode.SelectNodes("font/span");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode regNrNode = nodes[0];
        //                        animal.RegId = regNrNode.InnerText;
        //                    }
        //                    break;
        //                case 3:
        //                    nodes = tdNode.SelectNodes("font/span");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode node = nodes[0];
        //                        animal.Namn = node.InnerText.Trim();
        //                    }
        //                    break;
        //                case 4:
        //                    nodes = tdNode.SelectNodes("font/span");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode node = nodes[0];
        //                        animal.Kon = node.InnerText.Trim() == "T" ? Kon.Tik : Kon.Hund;
        //                    }
        //                    break;
        //                case 5:
        //                    nodes = tdNode.SelectNodes("font/span");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode node = nodes[0];
        //                        animal.Ras = node.InnerText.Trim();
        //                    }
        //                    break;
        //                case 6:
        //                    nodes = tdNode.SelectNodes("font/span");
        //                    if (nodes != null)
        //                    {
        //                        HtmlNode node = nodes[0];
        //                        animal.Saknad = node.InnerText == String.Empty ? false : true;
        //                    }
        //                    break;
        //            }
        //            columnIndex++;
        //        }

        //        resultList.animals.Add(animal);
        //    }

        //    return resultList;
        //}

        public Animal GetDogDetails(Animal dog, ViewState viewState) {

            // Make the request
            String response = _req.DoSpecDogRequest(dog, viewState);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            // TODO validate that we have a relevant page
            if (doc.DocumentNode != null)
            {
                return ParseDog(doc, dog);
            }

            throw new Exception("Ett fel uppstod när inläsning av hund genomfördes.");
        }

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

            return dog;
        } 

        // Find the id inside a span
        private String getValue(HtmlDocument dogTableNodes, String idToFind) {
            var node = dogTableNodes.DocumentNode.SelectNodes("//span[@id='" + idToFind + "']");
            if (node != null && node[0] != null)
            {
                return node[0].InnerText;
            }
            return "";
        }

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