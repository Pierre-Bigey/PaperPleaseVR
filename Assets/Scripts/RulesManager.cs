using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Entrants
{
    public class RuleSubject
    {
        public readonly bool country; //True if entrant's country, false if entrant's type
        public readonly List<Country> countriesSubjects;
        public readonly EntrantType entrantTypeSubject;

        public RuleSubject( List<Country> _countriesSubjects = null, EntrantType _entrantTypeSubject = EntrantType.WORKER, bool _country = true)
        {
            country = _country;
            countriesSubjects = _countriesSubjects;
            entrantTypeSubject = _entrantTypeSubject;
        }

        public bool IsConcerned(Country entrantOrigin, EntrantType entrantType)
        {
            if (country) return countriesSubjects.Contains(entrantOrigin);
            else return entrantTypeSubject == entrantType;
        }

    }
    
    public enum RuleSubjectShortcut{
        ALL,
        FOREIGNER,
        CITIZEN,
        SPEC_CITIZEN,
        SPEC_TYPE
    }

    public class RuleDocToPresent
    {
        

        public readonly RuleSubject subject;
        public readonly DocumentType docToPresent;
        public readonly string description;

        public RuleDocToPresent(string _description, RuleSubjectShortcut _subjectShortcut, DocumentType _docToPresent, List<Country> countriesSubjects = null,  EntrantType typesSubject = EntrantType.WORKER)
        {
            description = _description;
            
            switch (_subjectShortcut)
            {
                case RuleSubjectShortcut.ALL:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.ARSTOTZKA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.CITIZEN:
                    subject = new RuleSubject(new List<Country>() {Country.ARSTOTZKA, });
                    break;
                
                case RuleSubjectShortcut.FOREIGNER:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.SPEC_CITIZEN:
                    subject = new RuleSubject(countriesSubjects);
                    break;
                
                case RuleSubjectShortcut.SPEC_TYPE:
                    subject = new RuleSubject(null, typesSubject, false);
                    break;
                
            }

            docToPresent = _docToPresent;
        }
        
    }

    /// <summary>
    /// When a subject is concerned by this rules, he cannot pass the frontier
    /// </summary>
    public class RuleToEnter
    {
        public readonly RuleSubject subject;
        public readonly string description;

        public RuleToEnter(string _description, RuleSubjectShortcut _subjectShortcut, List<Country> countriesSubjects = null)
        {
            description = _description;
            switch (_subjectShortcut)
            {
                case RuleSubjectShortcut.ALL:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.ARSTOTZKA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.CITIZEN:
                    subject = new RuleSubject(new List<Country>() {Country.ARSTOTZKA, });
                    break;
                
                case RuleSubjectShortcut.FOREIGNER:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.SPEC_CITIZEN:
                    subject = new RuleSubject(countriesSubjects);
                    break;
                
                //This case shouldn't happen so I put it as citizen
                case RuleSubjectShortcut.SPEC_TYPE:
                    subject = new RuleSubject(new List<Country>() {Country.ARSTOTZKA, });
                    break;
                
            }
        }
    }
    
    
    public static class RulesManager
    {
        public static Dictionary<Country, string[]> IssuingCities = new()
        {
            {Country.ANTEGRIA, new string[] {"St. Marmero", "Glorian", "Outer Grouse"} },
            {Country.ARSTOTZKA, new string[] {"Orvech Vonor", "East Grestin", "Paradizna"} },
            {Country.IMPOR, new string[] {"Enkyo", "Haihan", "Tsunkeido"} },
            {Country.KOLECHIA, new string[] {"Yurko City", "Vedor", "West Grestin"} },
            {Country.OBRISTAN, new string[] {"Skal", "Lorndaz", "Mergerous"} },
            {Country.REPUBLIA, new string[] {"True Glorian", "Lesrenadi", "Bostan"} },
            {Country.UNITED_FEDERATION, new string[] {"Great Rapid", "Shingleton", "Korista City"} },
        };
        
        public static Dictionary<DocumentType, string> dateFormat = new()
        {
            { DocumentType.PASSPORT, "dd.MM.yyyy" },
            { DocumentType.ID_SUPPLEMENT, "dd.MM.yyyy" },
            { DocumentType.ACCESS_PERMIT, "dd.MM.yyyy" },
            { DocumentType.GRANT_OF_ASYLUM, "dd.MM.yyyy" },
            { DocumentType.ENTRY_PERMIT, "yyyy.MM.dd" },
            { DocumentType.ID_CARD, "yyyy.MM.dd" },
            { DocumentType.ENTRY_TICKET, "yyyy.MM.dd" },
            { DocumentType.CERTIF_OF_VACCINATION, "dd.MM.yy" },
        };

        public static List<RuleDocToPresent> RulesDocsToPresentsList = new List<RuleDocToPresent>()
        {
            new RuleDocToPresent("Entrant must have a passport", RuleSubjectShortcut.ALL, DocumentType.PASSPORT),//0
            new RuleDocToPresent("Foreigners require an entry ticket", RuleSubjectShortcut.FOREIGNER, DocumentType.ENTRY_TICKET),//1
            new RuleDocToPresent("Foreigners require an entry permit", RuleSubjectShortcut.FOREIGNER, DocumentType.ENTRY_PERMIT),//2
            new RuleDocToPresent("Arstotzkan citizens must have an ID card", RuleSubjectShortcut.CITIZEN, DocumentType.ID_CARD),//3
            new RuleDocToPresent("Workers must have a work pass", RuleSubjectShortcut.SPEC_TYPE, DocumentType.WORK_PASS,typesSubject:EntrantType.WORKER),//4
            new RuleDocToPresent("Diplomats require authorization", RuleSubjectShortcut.SPEC_TYPE, DocumentType.DIPLO_AUTH,typesSubject:EntrantType.DIPLOMAT),//5
            new RuleDocToPresent("Foreigners require an ID supplement", RuleSubjectShortcut.FOREIGNER, DocumentType.ID_SUPPLEMENT),//6
            new RuleDocToPresent("Asylum seekers must have a grant", RuleSubjectShortcut.SPEC_TYPE, DocumentType.GRANT_OF_ASYLUM,typesSubject:EntrantType.ASYLUM_SEEKER),//7
            new RuleDocToPresent("Entrant must have polio vaccine certificate", RuleSubjectShortcut.ALL, DocumentType.CERTIF_OF_VACCINATION),//8
            new RuleDocToPresent("Foreigners require an access permit", RuleSubjectShortcut.FOREIGNER, DocumentType.ACCESS_PERMIT),//9
        };

        public static Dictionary<int, List<RuleDocToPresent>> DocToPresentEachDay = new Dictionary<int, List<RuleDocToPresent>>()
        {
            {1,RulesDocsToPresentsList.GetRange(0,1)},
            {2,RulesDocsToPresentsList.GetRange(0,1)},
            {3,RulesDocsToPresentsList.GetRange(0,2)},
            {4,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3]}},
            {5,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3]}},
            {6,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4]}},
            {7,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4]}},
            {8,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5]}},
            {9,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5]}},
            {10,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5]}},
            {11,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5]}},
            {12,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5]}},
            {13,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {14,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {15,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {16,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {17,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {18,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {19,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {20,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6]}},
            {21,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7]}},
            {22,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7]}},
            {23,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7]}},
            {24,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7]}},
            {25,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7]}},
            {26,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[2],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[6],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
            {27,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[9],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
            {28,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[9],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
            {29,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[9],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
            {30,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[9],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
            {31,new List<RuleDocToPresent>(){RulesDocsToPresentsList[0],RulesDocsToPresentsList[9],RulesDocsToPresentsList[3],RulesDocsToPresentsList[4],RulesDocsToPresentsList[5],RulesDocsToPresentsList[7],RulesDocsToPresentsList[8]}},
        };
        
        public static List<RuleToEnter> RulesToEnterList = new List<RuleToEnter>()
        {
            new RuleToEnter("Arstotzkan citizens only", RuleSubjectShortcut.FOREIGNER),//0
            new RuleToEnter("No entry from Impor", RuleSubjectShortcut.SPEC_CITIZEN, new List<Country>(){Country.IMPOR }),//1
            new RuleToEnter("No entry from United Federation", RuleSubjectShortcut.SPEC_CITIZEN, new List<Country>(){Country.UNITED_FEDERATION }),//2
        };

        public static Dictionary<int, List<RuleToEnter>> CanEnterEachDay = new Dictionary<int, List<RuleToEnter>>()
        {
            { 1, RulesToEnterList.GetRange(0, 1) },
            { 19, RulesToEnterList.GetRange(1, 1) },
            { 25, RulesToEnterList.GetRange(2, 1) },
        };

    }
}