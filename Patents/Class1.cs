using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Patents
{
    class PatentFamily
    {
        public String title;
        public List<String> Patents;
        public String DWAN;
        public List<String> Applicants;
        public List<String> Citations;
        public List<String> Companies;
        public List<String> Countries;
        public List<String> IPCCodes;
        public int Year;
        public int Cited = 0;

        public PatentFamily(String line)
        {
            Patents = new List<string>();
            Applicants = new List<string>();
            Citations = new List<string>();
            Companies = new List<string>();
            Countries = new List<string>();
            IPCCodes = new List<string>();

            String[] splitString = line.Split('\t');
            title = splitString[1];
            Patents.AddRange(splitString[0].Replace(" ","").Split(';'));
            for (int i = 0; i < Patents.Count; i++)
            {
                String country = Patents[i].Substring(0, 2);
                if (!Countries.Contains(country))
                {
                    Countries.Add(country);
                }
            }

            String[] ipcCodes = splitString[10].Replace(" ","").Split(';');
            for (int i = 0; i < ipcCodes.Length; i++)
            {
                IPCCodes.Add(ipcCodes[i].Substring(0,3));
            }

            DWAN = splitString[4];
            Applicants.AddRange(splitString[2].Split(';'));

            Year = int.Parse(splitString[12].Split(' ').Last());

            String[] splitCitations = splitString[17].Split(';');
            foreach (String citation in splitCitations)
            {
                String firstWord = citation.Split(' ')[0];
                if (firstWord.Contains('-'))
                {
                    Citations.Add(firstWord);
                }
            }

            String[] splitCompanies = splitString[3].Split(';');
            foreach (String company in splitCompanies)
            {
                String name = company.Substring(company.IndexOf('(') + 1).Split(')')[0];
                if (!Companies.Contains(name) && !name.Contains("-Individual"))
                {
                    Companies.Add(name);
                }
            }
        }
    }
}
