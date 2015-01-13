using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKSearchAPI
{
    public enum Djurslag { Katt , Hund }

    public enum Kon { Hund = 0, Tik = 1, Hane = 2, Hona = 3 }

    public class AnimalList
    {
        public Djurslag Species { get; set; }
        public String errorMessage { get; set; }
        public ICollection<Animal> animals = new List<Animal>();
        public bool HasMoreThan20 { get; set; }
    }

    public class Animal
    {
        public Animal()
        {
            Agare = new Owner();
        }

        public String TatueringsId { get; set; }
        public String ChipId { get; set; }
        public String RegId { get; set; }
        public String DbId { get; set; }
        public String Namn { get; set; }
        public Djurslag Species { get; set; }
        public String Ras { get; set; }
        public Kon Kon { get; set; }
        public bool Saknad { get; set; }
        public String Harlag { get; set; }
        public String Farg { get; set; }
        public String Fodelsedatum { get; set; }
        public bool Kastrerad { get; set; }

        public Owner Agare { get; set; }

        public String linkNum { get; set; }

        /// <summary>
        /// Remove beginning and trailing spaces and all &nbsp;
        /// </summary>
        public void TrimStrings()
        {
            TatueringsId = TrimString(TatueringsId);
            ChipId = TrimString(ChipId);
            RegId = TrimString(RegId);
            DbId = TrimString(DbId);
            Namn = TrimString(Namn);
            Ras = TrimString(Ras);
            Harlag = TrimString(Harlag);
            Farg = TrimString(Farg);
            Fodelsedatum = TrimString(Fodelsedatum);

            Agare.TrimStrings();
        }

        private String TrimString(String toTrim)
        {
            return toTrim != null ? toTrim.Trim().Replace("&nbsp;", " ") : null;
        }
    }

    public class Owner
    {
        public String Namn { get; set; }
        public String Adress { get; set; }
        public String Epost { get; set; }
        public String TelHem { get; set; }
        public String TelArbete { get; set; }
        public String TelMobil { get; set; }

        /// <summary>
        /// Remove beginning and trailing spaces and all &nbsp;
        /// </summary>
        public void TrimStrings()
        {
            Namn = TrimString(Namn);
            Adress = TrimString(Adress);
            Epost = TrimString(Epost);
            TelArbete = TrimString(TelArbete);
            TelHem = TrimString(TelHem);
            TelMobil = TrimString(TelMobil);
        }

        private String TrimString(String toTrim)
        {
            return toTrim != null ? toTrim.Trim().Replace("&nbsp;", " ") : null;
        }
    }
}
