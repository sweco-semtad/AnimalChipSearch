using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Web.UI;
using SKKSearchAPI;

namespace SKKRegisterSok
{
    /// <summary>
    /// Parses the view state, constructing a viaully-accessible object graph.
    /// </summary>
    public class ViewStateParser
    {

        /// <summary>
        /// Parse animal object tree from view state
        /// </summary>
        /// <param name="viewStateString"></param>
        /// <param name="djurslag"></param>
        /// <returns></returns>
        public AnimalList ParseViewStateGraph(String viewStateString, Djurslag djurslag)
        {
            var animalList = new AnimalList();

            // First, deserialize the string into a Triplet
            LosFormatter los = new LosFormatter();
            Pair viewState = (Pair)los.Deserialize(viewStateString);

            ArrayList animalObjectTree = (ArrayList)((Pair)((ArrayList)((Pair)((ArrayList)((Pair)((ArrayList)((Pair)((Pair)viewState.First).Second).Second)[1]).Second)[7]).Second)[1]).Second;

            return djurslag == Djurslag.Hund ? ParseDogViewState(animalObjectTree) : ParseCatViewState(animalObjectTree);
        }

        /// <summary>
        /// Parse the animal object tree for dog search
        /// </summary>
        /// <param name="animalVSList"></param>
        /// <returns></returns>
        private AnimalList ParseDogViewState(ArrayList animalVSList)
        {
            var animalList = new AnimalList();
            animalList.Species = Djurslag.Hund;

            foreach (object obj in animalVSList)
            {
                if (obj is Pair)
                {
                    var animalTree = (ArrayList)((Pair)obj).Second;

                    var animal = new Animal();
                    animal.DbId =   (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[1]).Second)[1]).First).First)[1];

                    animal.ChipId = ((string[])((Pair)((ArrayList)((Pair)((ArrayList)((Pair)animalTree[3]).Second)[1]).Second)[1]).First)[0];

                    animal.RegId = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[5]).Second)[1]).First).First)[1];

                    animal.Namn = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[7]).Second)[1]).First).First)[1];

                    var kon = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[9]).Second)[1]).First).First)[1];
                    animal.Kon = kon == "T" ? Kon.Tik : Kon.Hund;

                    animal.Ras = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[11]).Second)[1]).First).First)[1];

                    animal.Species = Djurslag.Hund;

                    animal.TrimStrings();

                    animalList.animals.Add(animal);
                }
            }

            return animalList;
        }

        /// <summary>
        /// Parse the animal object tree for cat search
        /// </summary>
        /// <param name="animalVSList"></param>
        /// <returns></returns>
        private AnimalList ParseCatViewState(ArrayList animalVSList)
        {
            var animalList = new AnimalList();
            animalList.Species = Djurslag.Katt;

            foreach (object obj in animalVSList)
            {
                if (obj is Pair)
                {
                    var animalTree = (ArrayList)((Pair)obj).Second;

                    var animal = new Animal();
                    animal.DbId = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[1]).Second)[1]).First).First)[1];

                    animal.ChipId = ((string[])((Pair)((ArrayList)((Pair)((ArrayList)((Pair)animalTree[3]).Second)[1]).Second)[1]).First)[0];

                    animal.Namn = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[5]).Second)[1]).First).First)[1];

                    var kon = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[7]).Second)[1]).First).First)[1];
                    animal.Kon = kon == "Hane" ? Kon.Hane : Kon.Hona;

                    animal.Fodelsedatum = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[9]).Second)[1]).First).First)[1];

                    animal.Ras = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[11]).Second)[1]).First).First)[1];

                    animal.Farg = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[13]).Second)[1]).First).First)[1];

                    var agarOrt = (String)((ArrayList)((Pair)((Pair)((ArrayList)((Pair)animalTree[15]).Second)[1]).First).First)[1];

                    animal.Species = Djurslag.Katt;

                    animalList.animals.Add(animal);
                }
            }

            return animalList;
        }
    }
}