using System;
using SKKSearchAPI;

namespace AnimalChipSearch.ViewModels
{
    public class DjurslagViewModel
    {
        public Djurslag Djurslag { get; set; }

        public String Name {
            get {
                switch (Djurslag)
                {
                    case Djurslag.Hund :
                        return "Hund";
                    case Djurslag.Katt :
                        return "Katt";
                    default :
                        return "Tomt";
                }
            }
        }
    }
}
