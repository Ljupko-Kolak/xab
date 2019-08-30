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
            Console.WriteLine("30. 8. 2019.");
            string putanja = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OP podatci\\Resursi\\Baza.xml";

            //učitam bazu
            Baza baza = new Baza(putanja);

            Console.WriteLine("Pritisnite bilo koju tipku za početak...");
            Console.ReadKey(true);
            Console.WriteLine("\n");

            #region Korak 1
            Console.WriteLine("Mijenjanje telefonskog broja odjela Minimalno invazivne kirirgije");
            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Minimalno invazivna kirurgija")
                {
                    o.broj = "tel. 030/708-582";
                    break;
                }
            }
            Pohrani(baza);
            Console.WriteLine("Promjena obavljena!\n");
            #endregion

            #region Korak 2
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("Dodavanje doktora Ismara Rašića u odjele Kirurgija i Minimalno invazivna kirurgija");

            // napravim novog lijecnika
            Lijecnik ismar = new Lijecnik();
            ismar.imePrezime = "Ismar Rašić";
            ismar.titula1 = "dr.sc. Ismar Rašić";
            ismar.titula2 = "specijalist opće i subspecijalist abdominalne kirurgije";

            // dodavanje u oba odjela
            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Kirurgija" || o.naziv == "Minimalno invazivna kirurgija")
                {
                    bool containsDoctor = false;
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == ismar.imePrezime && l.titula1 == ismar.titula1 && l.titula2 == ismar.titula2)
                        {
                            containsDoctor = true;
                            break;
                        }
                    }
                    if (containsDoctor == false)
                    {
                        o.lijecnici.Add(ismar);
                        SortirajLijecnike(o);
                    }
                }
            }
            Pohrani(baza);
            Console.WriteLine("Liječnik unesen u bazu!\n");
            #endregion

            #region Korak 3
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("Dodavanje doktora Amira Jašarevića u odjel ORL i kirurgije glave i vrata)");

            // napravim novog lijecnika
            Lijecnik amir = new Lijecnik();
            amir.imePrezime = "Amir Jašarević";
            amir.titula1 = "dr. Amir Jašarević";
            amir.titula2 = "specijalist maksilofacijalne kirurgije";

            // dodavanje u odjel
            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Odjel otorinolaringologije i kirurgije glave i vrata")
                {
                    bool containsDoctor = false;
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == amir.imePrezime && l.titula1 == amir.titula1 && l.titula2 == amir.titula2)
                        {
                            containsDoctor = true;
                            break;
                        }
                    }
                    if (containsDoctor == false)
                    {
                        o.lijecnici.Add(amir);
                        SortirajLijecnike(o);
                    }
                }
            }
            Pohrani(baza);
            Console.WriteLine("Liječnik unesen u bazu!\n");
            #endregion

            #region Korak 4
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
            Console.ReadKey(true);
            Console.WriteLine("dr. Vesna Majher Tomić i dr. Tanja Jukić-Gavrić");

            foreach (Odjel o in baza.odjeli)
            {
                if (o.naziv == "Odjel anestezije i intenzivnog liječenja")
                {
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == "Vesna Majher")
                        {
                            l.imePrezime = "Vesna Majher Tomić";
                            l.titula1 = "dr.Vesna Majher Tomić";
                            break;
                        }
                    }

                    Lijecnik tanja = new Lijecnik();
                    tanja.imePrezime = "Tanja Jukić-Gavrić";
                    tanja.titula1 = "dr. Tanja Jukić-Gavrić";
                    tanja.titula2 = "specijalist anesteziologije, reanimatologije i intenzivnog liječenja";

                    bool containsDoctor = false;
                    foreach (Lijecnik l in o.lijecnici)
                    {
                        if (l.imePrezime == tanja.imePrezime && l.titula1 == tanja.titula1 && l.titula2 == tanja.titula2)
                        {
                            containsDoctor = true;
                            break;
                        }
                    }
                    if (containsDoctor == false)
                    {
                        o.lijecnici.Add(tanja);
                        SortirajLijecnike(o);
                    }
                    break;
                }
            }
            Pohrani(baza);
            Console.WriteLine("Promjene obavljene!");
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
