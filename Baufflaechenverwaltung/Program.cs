using System;
using System.Collections.Generic;

namespace Baufflaechenverwaltung
{
    public enum FlaechenStatus { Frei, Reserviert, Bebaut }
    public enum BauvorhabenStatus { AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen }

    public class Antragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;
    }

    public class Bauflaeche
    {
        public string Id { get; set; } = string.Empty;
        public double Groesse { get; set; }
        public string Lage { get; set; } = string.Empty;
        public string AktuelleNutzung { get; set; } = string.Empty;
        public string Bebaubarkeit { get; set; } = string.Empty;
        public string BPlanNummer { get; set; } = string.Empty;
        public decimal Bodenrichtwert { get; set; }
        public string Eigentuemer { get; set; } = string.Empty;
        public FlaechenStatus Status { get; set; } = FlaechenStatus.Frei;

        public void FlaecheReservieren()
        {
            if (Status == FlaechenStatus.Bebaut)
            {
                throw new InvalidOperationException($"Fläche {Id} kann nicht reserviert werden, da sie bereits bebaut ist.");
            }
            Status = FlaechenStatus.Reserviert;
        }
    }

    public class Grundstueck
    {
        public string Flurstuecknummer { get; set; } = string.Empty;
        public List<Bauflaeche> Bauflaechen { get; set; } = new List<Bauflaeche>();
    }

    public class Bauvorhaben
    {
        public string Titel { get; set; } = string.Empty;
        public Antragsteller Antragsteller { get; set; } = new Antragsteller();
        public string GeplanteNutzung { get; set; } = string.Empty;
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public BauvorhabenStatus Status { get; set; } = BauvorhabenStatus.AntragEingereicht;
        public List<Bauflaeche> ZugeordneteFlaechen { get; set; } = new List<Bauflaeche>();

        public void ZuweiseFlaeche(Bauflaeche flaeche)
        {
            if (flaeche.Status == FlaechenStatus.Bebaut)
            {
                throw new InvalidOperationException($"Fläche {flaeche.Id} ist bereits bebaut und kann keinem neuen Bauvorhaben zugewiesen werden.");
            }
            ZugeordneteFlaechen.Add(flaeche);
        }

        public void StatusAktualisieren(BauvorhabenStatus neuerStatus)
        {
            Status = neuerStatus;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Demonstration der Funktionalität
            var grundstueck = new Grundstueck { Flurstuecknummer = "0015 00012 001/002" };
            var flaeche1 = new Bauflaeche 
            {
                Id = "F1", 
                Groesse = 500, 
                Lage = "Nordseite", 
                AktuelleNutzung = "Brachfläche", 
                Bebaubarkeit = "ja", 
                BPlanNummer = "BP-2022-089", 
                Bodenrichtwert = 500m, 
                Eigentuemer = "Max Mustermann",
                Status = FlaechenStatus.Bebaut
            };
            grundstueck.Bauflaechen.Add(flaeche1);

            var antragsteller = new Antragsteller { Name = "Erika Musterfrau", Firma = "Bau AG", Kontaktdaten = "erika@bauag.de" };
            var vorhaben = new Bauvorhaben 
            {
                Titel = "Wohnhaus Nord", 
                Antragsteller = antragsteller, 
                GeplanteNutzung = "Wohngebäude", 
                Beginn = DateTime.Now.AddMonths(1), 
                Fertigstellung = DateTime.Now.AddMonths(12)
            };

            Console.WriteLine("Test: Reservierung einer bebauten Fläche...");
            try
            {
                flaeche1.FlaecheReservieren();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }

            Console.WriteLine("\nTest: Zuweisung einer bebauten Fläche zu Bauvorhaben...");
            try
            {
                vorhaben.ZuweiseFlaeche(flaeche1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
    }
}