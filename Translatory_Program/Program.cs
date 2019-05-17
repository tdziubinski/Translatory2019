using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translatory_Tokeny_2403
{
    public class Wyrazenia
    {
        public string typ { get; set; }
        public string wartosc { get; set; }
    }

    class Program
    {

        public static List<Wyrazenia> wyrazeniaLista = new List<Wyrazenia>();//docelowa
        public static List<Wyrazenia> inneLista = new List<Wyrazenia>();//dla spacji itp

        public static string example3;
        static void Main(string[] args)
        {
            example3 = @"x12  -38948- (-44)  ( -44.55.66) 3 7* ! Zmienna5445gfg d    

            d         67576h   
            ";
         
            // '+', '*', '-', '/', '(', ')');

            string exampleToDisplay = example3;

            //example3= example3.Replace(" ","");

            int i = 0;

            while (i < example3.Length)
            {

                string result = "";

                if (example3[i] == '+' || example3[i] == '*' || example3[i] == '-' || example3[i] == '/')
                {
                    if (example3[i] == '-' && Char.IsNumber(example3[i + 1]))
                    {
                        if (CheckMinusIntOrDouble())
                        { result = CheckNumber(); i = 0; }
                        else
                        {
                            DodajOperator(i);
                            result = example3[i].ToString(); i = 0;
                        }
                    }
                    else if (example3[i] == '-' && Char.IsWhiteSpace(example3[i + 1]))
                    {
                        if (CheckAfterWhitespace())
                        {
                            if (CheckMinusIntOrDouble())
                            { result = CheckNumber(); i = 0; }
                            else
                            {
                                DodajOperator(i);
                                result = example3[i].ToString(); i = 0;
                            }
                        }
                        else
                        {
                            DodajOperator(i);
                            result = example3[i].ToString(); i = 0;
                        }
                    }
                    // ; && Char.IsNumber(example3[i + 1]))  { result = CheckNumber(); i = 0; }
                    else
                    {
                        DodajOperator(i);
                        result = example3[i].ToString(); i = 0;
                    }
                }
                else if (example3[i] == '(' || example3[i] == ')')
                {
                    Wyrazenia wyrazenia = new Wyrazenia();
                    wyrazenia.typ = "nawias";
                    wyrazenia.wartosc = example3[i].ToString();
                    wyrazeniaLista.Add(wyrazenia);
                    result = example3[i].ToString(); i = 0;
                }
                else if (example3[i] == ' ') { result = CheckSpaces(); i = 0; }
                else if (Char.IsLetter(example3[i])) { result = CheckLetter(); i = 0; }
                else if (Char.IsNumber(example3[i])) { result = CheckNumber(); i = 0; }
                else
                {
                    Wyrazenia wyrazenia = new Wyrazenia();
                    wyrazenia.typ = "blad";
                    wyrazenia.wartosc = example3[i].ToString();
                    wyrazeniaLista.Add(wyrazenia);
                    result = example3[i].ToString(); i = 0;
                }

                if (result.Length > 0) { example3 = example3.Substring(result.Length); continue; }
                i++;

            }

            Console.WriteLine("Przykład: -->" + exampleToDisplay + "<--");
            Console.WriteLine("=============== Poszczegolne wyrazenia w podanej kolejnosci (wyszczegolnione typy): ====================");
            foreach (var item in wyrazeniaLista)
            {
                Console.WriteLine(item.typ + ": " + item.wartosc);
            }
            Console.Read();
        }
        public static void DodajOperator(int i)
        {
            Wyrazenia wyrazenia = new Wyrazenia();
            wyrazenia.typ = "operator";
            wyrazenia.wartosc = example3[i].ToString();
            wyrazeniaLista.Add(wyrazenia);
        }
        public static string CheckSpaces()
        {
            Regex reg = new Regex(@"\s+");//do znalezienia
            string result = reg.Match(example3).Value;
            Wyrazenia wyrazenia = new Wyrazenia();
            wyrazenia.typ = "spacja";
            wyrazenia.wartosc = result;
            inneLista.Add(wyrazenia);

            return result;
        }
        public static bool CheckAfterWhitespace()
        {
            var tempex3 = example3;
            char[] tempex4 = tempex3.Replace(" ", "").ToCharArray();
            if (Char.IsNumber(tempex4.FirstOrDefault())) return true;
            return false;
        }


        public static bool CheckMinusIntOrDouble()
        {
            if (wyrazeniaLista.Count == 0) return true;
            if (wyrazeniaLista.Last().typ == "nawias" || wyrazeniaLista.Last().typ == "operator")
            {
                return true;

            }

            return false;
        }
        public static string CheckLetter()
        {
            //Regex reg = new Regex(@"([a-zA-Z]+\d+)|([a-zA-Z]+)");//do znalezienia
            Regex reg = new Regex(@"([a-zA-Z]+\w+)|([a-z]){1,}");
            string result = "";
            result = reg.Match(example3).Value;



            Wyrazenia wyrazenia = new Wyrazenia();
            wyrazenia.typ = "identyfikator";
            wyrazenia.wartosc = result;
            wyrazeniaLista.Add(wyrazenia);

            return result;
        }

        public static string CheckNumber()
        {
            bool isDouble = false;
            //Regex reg = new Regex(@"-\s+\d+|\d+");
            Regex reg = new Regex(@"-\d+|\d+");

            string result1 = reg.Match(example3).Value;//dodanie do listy

            string example4 = example3.Substring(result1.Length);//ucina pierwsza wartosc int        

            if (example4.Length > 1)
            { if (example4[0] == '.' && char.IsNumber(example4[1])) { isDouble = true; } }


            if (isDouble)
            {

                //Regex reg1 = new Regex(@"-?\d+(?:\.\d+)?");
                Regex reg1 = new Regex(@"-?\d+(?:\.\d+)?");
                string result = reg1.Match(example3).Value;//tylko pierwszy

                Wyrazenia wyrazenia = new Wyrazenia();
                wyrazenia.typ = "double";
                wyrazenia.wartosc = result;
                wyrazeniaLista.Add(wyrazenia);
                return result;
            }

            Wyrazenia wyrazenia1 = new Wyrazenia();
            wyrazenia1.typ = "int";
            wyrazenia1.wartosc = result1;
            wyrazeniaLista.Add(wyrazenia1);

            return result1;
        }


    }
}