using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Patents
{
    public partial class Form1 : Form
    {
        private List<PatentFamily> Families;
        
        public Form1()
        {
            InitializeComponent();

            Families = new List<PatentFamily>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Re-initialise family collection
            this.Families = new List<PatentFamily>();
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //for each selected file
                foreach (String file in openFileDialog1.FileNames)
                {
                    //read all lines
                    String[] lines = File.ReadAllLines(openFileDialog1.FileName);
                    
                    //parse all lines as patent families
                    for (int i = 1; i < lines.Length; i++)
                    {
                        Families.Add(new PatentFamily(lines[i]));
                    }

                }

                //Initialise output buffer
                List<String> newLines = new List<string>();
                
                //Fill output buffer
                newLines = getYearStats();

                //newLines = getCitationQuery();

                //newLines = getCompanyStats1();

                //newLines = getCompanyStats2();

                //newLines = getCountryStats1();

                //newLines = getCountryStats2();

                //newLines = getTopicStats();

                //Write output and open it in notepad
                File.WriteAllLines("output.txt", newLines.ToArray());

                Process.Start("output.txt");
            }
        }

        private List<String> getCountryStats1()
        {
            List<String> lines = new List<string>();

            Dictionary<String, int> patentsbycountry = new Dictionary<string, int>();

            foreach (PatentFamily pf in Families)
            {
                foreach (String c in pf.Countries)
                {
                    if (patentsbycountry.Keys.Contains(c))
                    {
                        patentsbycountry[c]++;
                    }
                    else
                    {
                        patentsbycountry[c] = 1;
                    }
                }
            }

            foreach (KeyValuePair<String, int> entry in patentsbycountry)
            {
                lines.Add(entry.Key + "\t" + entry.Value);
            }

            return lines;
        }

        private List<String> getCountryStats2()
        {
            List<String> lines = new List<string>();

            int LowYear = 2013;
            int highYear = 2013;

            foreach (PatentFamily pf in Families)
            {
                if (pf.Year < LowYear)
                {
                    LowYear = pf.Year;
                }
                else if (pf.Year > highYear)
                {
                    highYear = pf.Year;
                }
            }

            Dictionary<String, int[]> patentsbycountry = new Dictionary<string, int[]>();

            foreach (PatentFamily pf in Families)
            {
                foreach (String c in pf.Countries)
                {
                    if (!patentsbycountry.Keys.Contains(c))
                    {
                        patentsbycountry[c] = new int[highYear - LowYear + 1];
                    }

                    patentsbycountry[c][pf.Year - LowYear]++;
                }
            }

            foreach (KeyValuePair<String, int[]> entry in patentsbycountry)
            {
                lines.Add(entry.Key);
                for (int i = 0; i < entry.Value.Length; i++)
                {
                    //if (entry.Value[i] > 0)
                   // {
                        lines.Add("\t" + (i + LowYear) + "\t" + entry.Value[i]);
                    //}
                }
            }

            return lines;
        }

        private List<String> getCompanyStats2()
        {
            List<String> lines = new List<string>();

            int LowYear = 2013;
            int highYear = 2013;

            foreach (PatentFamily pf in Families)
            {
                if (pf.Year < LowYear)
                {
                    LowYear = pf.Year;
                }
                else if (pf.Year > highYear)
                {
                    highYear = pf.Year;
                }
            }

            Dictionary<String, int[]> patentsbycompany = new Dictionary<string, int[]>();

            foreach (PatentFamily pf in Families)
            {
                foreach (String c in pf.Companies)
                {
                    if (!patentsbycompany.Keys.Contains(c))
                    {
                        patentsbycompany[c] = new int[highYear - LowYear + 1];
                    }

                    patentsbycompany[c][pf.Year - LowYear]++;
                }
            }

            foreach (KeyValuePair<String, int[]> entry in patentsbycompany)
            {
                lines.Add(entry.Key);
                for (int i = 0; i < entry.Value.Length; i++)
                {
                    //if (entry.Value[i] > 0)
                    //{
                        lines.Add("\t" + (i + LowYear) + "\t" + entry.Value[i]);
                    //}
                }
            }

            return lines;
        }

        private List<String> getCompanyStats1()
        {
            List<String> lines = new List<string>();

            Dictionary<String, int> patentsbycompany = new Dictionary<string, int>();

            foreach (PatentFamily pf in Families)
            {
                foreach (String c in pf.Companies)
                {
                    if (patentsbycompany.Keys.Contains(c)){
                        patentsbycompany[c]++;
                    }
                    else
                    {
                        patentsbycompany[c] = 1;
                    }
                }
            }

            foreach (KeyValuePair<String, int> entry in patentsbycompany)
            {
                lines.Add(entry.Key+"\t"+entry.Value);
            }

            return lines;
        }

        private List<String> getYearStats()
        {
            List<String> lines = new List<string>();

            int LowYear = 2013;
            int highYear = 2013;

            foreach (PatentFamily pf in Families)
            {
                if (pf.Year < LowYear)
                {
                    LowYear = pf.Year;
                }
                else if (pf.Year > highYear)
                {
                    highYear = pf.Year;
                }
            }

            int[] Years = new int[highYear - LowYear + 1];

            foreach (PatentFamily pf in Families)
            {
                Years[pf.Year - LowYear]++;
            }


            for (int i = 0; i < Years.Length; i++)
            {
                lines.Add((LowYear + i) + "\t" + Years[i]);
            }

            return lines;
        }

        private List<String> getCitationQuery()
        {
            List<String> lines = new List<string>();

            for (int i = 0; i < (Families.Count / 50) + 1; i++)
            {
                String searchString = "CD=(";
                int end = i == 2 ? Families.Count % 50 : 50;
                for (int j = 0; j < end; j++)
                {
                    searchString += Families[i * 50 + j].DWAN;
                    if (j < end - 1)
                    {
                        searchString += " OR ";
                    }
                }
                searchString += ")";
                lines.Add(searchString);
            }

            return lines;
        }

        private List<string> getTopicStats()
        {
            List<string> lines = new List<string>();

            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (PatentFamily pf in Families)
            {
                foreach (String c in pf.IPCCodes)
                {
                    if (!dict.Keys.Contains(c))
                    {
                        dict[c] = 0;
                    }

                    dict[c]++;
                }
            }

            foreach (KeyValuePair<string, int> entry in dict)
            {
                lines.Add(entry.Key + "\t" + entry.Value);
            }

            return lines;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                foreach (String filename in openFileDialog2.FileNames)
                {
                    String[] lines = File.ReadAllLines(filename);
                    for (int i = 1; i < lines.Length; i++)
                    {
                        String[] splitString = lines[i].Split('\t');
                        
                        String[] splitCitations = splitString[17].Split(';');

                        Regex regex = new Regex(@"\s(\w+-\w{1,2})\s");
                        Match m = regex.Match(splitString[17]);

                        foreach (Capture c in m.Groups)
                        {
                            for (int j = 0; j < Families.Count; j++)
                            {
                                if (Families[j].Patents.Contains(c.Value))
                                {
                                    Families[j].Cited++;
                                }
                            }
                        }
                    }
                }

                List<String> output = new List<string>();
                output.Add("Titel \t Grootte familie \t Aantal keer geciteerd");
                for (int i = 0; i < Families.Count; i++)
                {
                    output.Add(Families[i].title+"\t"+Families[i].Patents.Count+"\t"+Families[i].Cited);
                }

                File.WriteAllLines("result-indv.txt", output.ToArray());

                Dictionary<String, int> totalFamilySize = new Dictionary<string, int>();
                Dictionary<String, int> totalCited = new Dictionary<string, int>();

                for (int i = 0; i < Families.Count; i++)
                {
                    for (int j = 0; j < Families[i].Companies.Count; j++)
                    {
                        if (!totalCited.Keys.Contains(Families[i].Companies[j]))
                        {
                            totalFamilySize[Families[i].Companies[j]] = 0;
                            totalCited[Families[i].Companies[j]] = 0;
                        }
                        totalFamilySize[Families[i].Companies[j]] += Families[i].Patents.Count;
                        totalCited[Families[i].Companies[j]] += Families[i].Cited;
                    }
                }

                List<String> output2 = new List<string>();
                output2.Add("Bedrijfsafkorting \t Totaal aantal patenten \t Totaal aantal keer geciteerd");
                for (int i=0;i<totalCited.Count;i++)
                {
                    String key = totalCited.Keys.ElementAt(i);
                    output2.Add(key+"\t"+totalFamilySize[key]+"\t"+totalCited[key]);
                }

                File.WriteAllLines("result-comp.txt", output2.ToArray());
            }*/
        }
    }
}
