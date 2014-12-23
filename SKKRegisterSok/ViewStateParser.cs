using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Web.UI;

namespace SKKRegisterSok
{
    /// <summary>
    /// Parses the view state, constructing a viaully-accessible object graph.
    /// </summary>
    public class ViewStateParser
    {
        public AnimalList ParseViewStateGraph(String viewStateString)
        {
            var animalList = new AnimalList();

            // First, deserialize the string into a Triplet
            LosFormatter los = new LosFormatter();
            Pair viewState = (Pair)los.Deserialize(viewStateString);

            ArrayList animalObjectTree = (ArrayList)((Pair)((ArrayList)((Pair)((ArrayList)((Pair)((ArrayList)((Pair)((Pair)viewState.First).Second).Second)[1]).Second)[7]).Second)[1]).Second;

            return ParseAnimalViewState(animalObjectTree);
        }

        private AnimalList ParseAnimalViewState(ArrayList animalVSList)
        {
            var animalList = new AnimalList();
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

                    animalList.animals.Add(animal);
                }
            }

            return animalList;
        }
    }
}