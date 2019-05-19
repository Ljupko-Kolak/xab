using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AzuriranjeBaze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AŽURIRANJE BAZE\n");
            Console.WriteLine("5. 9. 2019.");
            Console.WriteLine("1)\tBrisanje dr.Mladena Čuturića i Prim.dr.Petra Lekića");
            Console.WriteLine("2)\tDodavanje odjela Minimalno invazivne kirurgije i");
            Console.WriteLine("\tkopiranje liječnika sa odjela intenzivnog liječenja i kirurgije u novonastali odjel");
            Console.WriteLine("3)\tDodavanje odjela ORL i kirurgije glave i vrata i");
            Console.WriteLine("\tpostavljanje dr.Mladena Čuturića kao šefa novonastalog odjela");
            Console.WriteLine("4)\tPostavljanje dr.Marka Androševića kao šefa odjela intenzivnog liječenja");
            Console.WriteLine("5)\tPostavljanje dr.Zvonimira Stipca kao šefa odjela ginekologije");
            Console.WriteLine("\n");
            string putanja = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OP podatci\\Resursi\\Baza.xml";

            //učitam bazu
            Baza baza = new Baza(putanja);

            Console.WriteLine("Pritisnite bilo koju tipku za početak...");
            Console.ReadKey(true);
            Console.WriteLine("\n");

            #region Korak 1
            Console.WriteLine("1)");
            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Kirurgija")
                {
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == "Mladen Čuturić")
                        {
                            o.lijecnici.Remove(l);
                            break;
                        }
                    }
                }
                else if (o.naziv == "Odjel anestezije i intenzivnog liječenja")
                {
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == "Petar Lekić")
                        {
                            o.lijecnici.Remove(l);
                            break;
                        }
                    }
                }
            }
            Pohrani(baza);
            Console.WriteLine("Dr.Mladen Čuturić i dr.Petar Lekić uspješno izbrisani!");
            #endregion

            #region Korak 2
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("2)");

            //napravim novi odjel
            Odjel mik = new Odjel();
            mik.naziv = "Minimalno invazivna kirurgija";
            mik.broj = "tel. 030/708-600";
            mik.sef = "Dalibor Kolak";
            mik.titula1 = "dr.Dalibor Kolak";
            mik.titula2 = "specijalist opće kirurgije";

            //dodaj liječnike u odjel
            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Interno" || o.naziv == "Kirurgija")
                {
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (mik.lijecnici.Contains(l) == false)
                        {
                            mik.lijecnici.Add(l);
                        }
                    }
                }
            }

            SortirajLijecnike(mik);

            //izvršavanje funkcije za dodavanje novog odjela
            DodajOdjel(baza, mik);
            #endregion

            #region Korak 3
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("3)");

            //napravim novi odjel
            Odjel orlgv = new Odjel();
            orlgv.naziv = "Odjel otorinolaringologije i kirurgije glave i vrata";
            orlgv.broj = "tel. 030/708-601";
            orlgv.sef = "Mladen Čuturić";
            orlgv.titula1 = "dr.Mladen Čuturić";
            orlgv.titula2 = "specijalist otorinolaringologije";

            //dodaj liječnike u odjel
            List<Lijecnik> lijecnici = new List<Lijecnik>();

            Lijecnik noviLijecnik = new Lijecnik();

            noviLijecnik.imePrezime = "Mladen Čuturić";
            noviLijecnik.titula1 = "dr.Mladen Čuturić";
            noviLijecnik.titula2 = "specijalist otorinolaringologije";

            lijecnici.Add(noviLijecnik);

            orlgv.lijecnici = lijecnici;

            //SortirajLijecnike(orlgv);

            //izvršavanje funkcije za dodavanje novog odjela
            DodajOdjel(baza, orlgv);
            #endregion

            #region Korak 4
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("4)");

            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Odjel anestezije i intenzivnog liječenja")
                {
                    o.sef = "Marko Androšević";
                    o.titula1 = "dr.Marko Androšević";
                    o.titula2 = "specijalist anesteziologije, reanimatologije i intenzivnog liječenja";
                    break;
                }
            }
            Pohrani(baza);
            Console.WriteLine("Dr.Marko Androšević postavljen kao šef odjela anestezije i intenzivnog liječenja");
            #endregion

            #region Korak 5
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("5)");

            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Ginekologija")
                {
                    o.sef = "Zvonimir Stipac";
                    o.titula1 = "dr.Zvonimir Stipac";
                    o.titula2 = "specijalist ginekologije i porodništva";
                    break;
                }
            }
            Pohrani(baza);
            Console.WriteLine("Dr.Zvonimir Stipac postavljen kao šef odjela ginekologije");
            #endregion

            Console.WriteLine("Pritisnite bilo koju tipku za izlaz...");
            Console.ReadKey(true);
        }

        static void SortirajLijecnike(Odjel odjel)
        {
            for (int i = 0; i < odjel.lijecnici.Count; i++)
            {
                for (int j = i; j < odjel.lijecnici.Count; j++)
                {
                    if (odjel.lijecnici[j].imePrezime.CompareTo(odjel.lijecnici[i].imePrezime) < 0)
                    {
                        Lijecnik temp = odjel.lijecnici[i];
                        odjel.lijecnici[i] = odjel.lijecnici[j];
                        odjel.lijecnici[j] = temp;
                    }
                }
            }
        }
        static void DodajOdjel(Baza baza, Odjel novi)
        {
            //provjeravam da li "novi" odjel već postoji
            bool noviOdjelPostoji = false;

            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == novi.naziv)
                {
                    noviOdjelPostoji = true;
                }
            }

            if (noviOdjelPostoji)
            {
                Console.WriteLine("Odjel '"+ novi.naziv + "' već postoji!");
            }
            else
            {
                Console.WriteLine("Unos novog odjela '"+ novi.naziv + "'");

                //dodaj odjel u bazu
                baza.odjeli.Add(novi);
                SortirajOdjele(baza);

                Pohrani(baza);
                Console.WriteLine("Odjel uspješno dodan!");
            }
        }
        static void SortirajOdjele(Baza baza)
        {
            for (int i = 0; i < baza.odjeli.Count; i++)
            {
                for (int j = i; j < baza.odjeli.Count; j++)
                {
                    if (baza.odjeli[j].naziv.CompareTo(baza.odjeli[i].naziv) < 0)
                    {
                        Odjel temp = baza.odjeli[i];
                        baza.odjeli[i] = baza.odjeli[j];
                        baza.odjeli[j] = temp;
                    }
                }
            }
        }

        static void Pohrani(Baza azuriranaBaza)
        {
            XDocument doc = XDocument.Load(azuriranaBaza.path);

            XElement bazaOdjela = doc.Descendants("odjeli").First();
            bazaOdjela.Elements().Remove();
            foreach (Odjel o in azuriranaBaza.odjeli)
            {
                XElement noviOdjel = new XElement("odjel");
                noviOdjel.Add(new XAttribute("naziv", o.naziv));
                noviOdjel.Add(new XAttribute("broj", o.broj));
                noviOdjel.Add(new XAttribute("sef", o.sef));
                noviOdjel.Add(new XAttribute("titula1", o.titula1));
                noviOdjel.Add(new XAttribute("titula2", o.titula2));

                XElement lijecnici = new XElement("lijecnici");
                foreach (Lijecnik l in o.lijecnici)
                {
                    XElement noviLijecnik = new XElement("lijecnik", l.imePrezime);
                    noviLijecnik.Add(new XAttribute("titula1", l.titula1));
                    noviLijecnik.Add(new XAttribute("titula2", l.titula2));

                    lijecnici.Add(noviLijecnik);
                }
                noviOdjel.Add(lijecnici);
                bazaOdjela.Add(noviOdjel);
            }

            azuriranaBaza.odjelniLijecnici.Sort();
            XElement bazaOdjelnihLijecnika = doc.Descendants("odjelnilijecnici").First();
            bazaOdjelnihLijecnika.Elements().Remove();
            foreach (string lijecnik in azuriranaBaza.odjelniLijecnici)
            {
                bazaOdjelnihLijecnika.Add(new XElement("lijecnik", lijecnik));
            }

            azuriranaBaza.imena.Sort();
            XElement bazaImena = doc.Descendants("imena").First();
            bazaImena.Elements().Remove();
            foreach (string ime in azuriranaBaza.imena)
            {
                bazaImena.Add(new XElement("ime", ime));
            }

            azuriranaBaza.prezimena.Sort();
            XElement bazaPrezimena = doc.Descendants("prezimena").First();
            bazaPrezimena.Elements().Remove();
            foreach (string prezime in azuriranaBaza.prezimena)
            {
                bazaPrezimena.Add(new XElement("prezime", prezime));
            }

            azuriranaBaza.gradovi.Sort();
            XElement bazaGradova = doc.Descendants("gradovi").First();
            bazaGradova.Elements().Remove();
            foreach (string grad in azuriranaBaza.gradovi)
            {
                bazaGradova.Add(new XElement("grad", grad));
            }

            azuriranaBaza.ulice.Sort();
            XElement bazaUlica = doc.Descendants("ulice").First();
            bazaUlica.Elements().Remove();
            foreach (string ulica in azuriranaBaza.ulice)
            {
                bazaUlica.Add(new XElement("ulica", ulica));
            }

            azuriranaBaza.dijagnoze.Sort();
            XElement bazaDijagnoza = doc.Descendants("dijagnoze").First();
            bazaDijagnoza.Elements().Remove();
            foreach (string dijagnoza in azuriranaBaza.dijagnoze)
            {
                bazaDijagnoza.Add(new XElement("dijagnoza", dijagnoza));
            }

            azuriranaBaza.zahvati.Sort();
            XElement bazaZahvata = doc.Descendants("zahvati").First();
            bazaZahvata.Elements().Remove();
            foreach (string zahvat in azuriranaBaza.zahvati)
            {
                bazaZahvata.Add(new XElement("zahvat", zahvat));
            }

            doc.Save(azuriranaBaza.path);
        }
    }
}
