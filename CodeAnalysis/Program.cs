using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CodeAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var filePath = Path.Combine(projectDirectory, "books.xml");

            var elementName = "book";
            var attributeName = "id";

            string attrValueOld = null;
            try
            {
                attrValueOld = Func1(filePath, elementName, attributeName);
                Console.WriteLine($"Func1(): '{attrValueOld}'");
            }
            catch
            {
                Console.WriteLine("Initial function throws");
            }

            var attrValueNew = GetXmlAttributeValue(filePath, elementName, attributeName);
            Console.WriteLine($"GetXmlAttributeValue(): '{attrValueNew}'");
        }

        // Плохое имя функции, плохое имя первого аргумента.
        static string Func1(string input, string elementName, string attrName)
        {
            string[] lines = System.IO.File.ReadAllLines(input);

            // Для хранения промежуточного результата лучше использовать StringBuilder.
            string result = null;
            foreach (var line in lines)
            {
                // Плохое имя переменной startElEndex.
                // Некорректное определение наличия элемента в строке,
                // в данном случае startElEndex может указывать на элемент чье имя содержит elementName как подстроку.
                var startElEndex = line.IndexOf(elementName);
                if (startElEndex != -1)
                {
                    if (line[startElEndex - 1] == '<')

                    {
                        // Некорректное определения конца элемента, символ ">" может встретится и в теле самого элемента.
                        var endElIndex = line.IndexOf('>', startElEndex - 1);

                        // Некорретный поиск имени атрибута.
                        var attrStartIndex = line.IndexOf(attrName, startElEndex, endElIndex - startElEndex + 1);

                        if (attrStartIndex != -1)
                        {
                            // Магическое число 2.
                            int valueStartIndex = attrStartIndex + attrName.Length + 2;

                            // Значение аттрибута может быть заключенно в одинарные кавычки в таком случае
                            // цикл завершится исключением System.IndexOutOfRangeException.
                            while (line[valueStartIndex] != '"')

                            {
                                // Слишком большая вложеность кода.
                                // Выделение новой строки на каждой итерации, ненужная нагрузка на GC.
                                result += line[valueStartIndex];

                                valueStartIndex++;
                            }

                            break;
                        }
                    }
                }
            }

            return result;
        }

        // Для работы с XML предпочтительный способ это Linq to XML.
        static string GetXmlAttributeValue(string filePath, string elementName, string attrName)
        {
            var root = XElement.Load(filePath);
            var value = root
                .Elements(elementName)
                .FirstOrDefault(el => el.Attribute(attrName) != null)
                ?.Attribute(attrName)
                ?.Value;

            return value;
        }
    }
}