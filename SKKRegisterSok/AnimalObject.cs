using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKRegisterSok
{
    public class AnimalList
    {
        public String errorMessage { get; set; }
        public ICollection<Animal> animals = new List<Animal>();
        public ViewState viewState = new ViewState();
    }

    public enum Kon { Hund = 0, Tik = 1}

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
        public String Ras { get; set; }
        public Kon Kon { get; set; }
        public bool Saknad { get; set; }
        public String Harlag { get; set; }
        public String Farg { get; set; }
        public String Fodelsedatum { get; set; }
        public Owner Agare { get; set; }

        public String linkNum { get; set; }
    }

    public class Owner
    {
        public String Namn { get; set; }
        public String Adress { get; set; }
        public String Epost { get; set; }
        public String TelHem { get; set; }
        public String TelArbete { get; set; }
        public String TelMobil { get; set; }
    }
}
