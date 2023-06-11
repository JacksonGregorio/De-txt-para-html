
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace projeto_nelson
{
    internal class Program
    {
        static string s = "cod:space";

        static string returnLineHtml(string content, string typeTag) {
            string linhaHTML = "";

            if(typeTag == "button")
                linhaHTML = System.Text.RegularExpressions.Regex.Replace(content, @"button:(\w+)", $@"<input{s}type=""submit""{s}title=""$1"">");
            else if(typeTag == "text")
                linhaHTML = System.Text.RegularExpressions.Regex.Replace(content, @"text:(\w+)", $@"$1:{s}<input{s}type=""text"">");
            else if(typeTag == "combobox")
            {
                string pattern = @"combobox:\[(.*?)\]";
    
                linhaHTML = Regex.Replace(content, pattern, match =>
                {
                    string optionsStr = match.Groups[1].Value;
                    string[] options = optionsStr.Split(',');
                    
                    string selectContent = "<select>";
                    foreach (string option in options)
                    {
                        string[] parts = option.Split(':');
                        if (parts.Length == 2)
                        {
                            selectContent += "  <option>" + parts[1].Trim() + "</option>";
                        }
                    }
                    selectContent += "</select>";
                    
                    return selectContent;
                });
            }
            
            return linhaHTML;
        }

        static string fixLineSpacement(string lineHtml) {
            return lineHtml.Replace(" ", "&nbsp;&nbsp;").Replace(s, " ");
        }

        static void Main(string[] args)
        {
            string arquivoTxt = "./public/teste.txt";

            // Caminho do arquivo de saída HTML
            string arquivoHtml = "./public/index.html";

            try
            {
                // Ler o arquivo TXT
                string[] linhas = File.ReadAllLines(arquivoTxt);

                // Criar o arquivo HTML
                using (StreamWriter writer = new StreamWriter(arquivoHtml))
                {
                    // Escrever o início do documento HTML
                    writer.WriteLine("<!DOCTYPE html>");
                    writer.WriteLine("<html>");
                    writer.WriteLine("<head>");
                    writer.WriteLine("<title>Arquivo HTML gerado</title>");
                    writer.WriteLine("</head>");
                    writer.WriteLine("<body>");

                    writer.WriteLine("<form>");
                    // Iterar sobre as linhas do arquivo TXT
                    foreach (string linha in linhas)
                    {
                        Console.WriteLine(linha);
                        // Remover espaços em branco no início e no fim da linha
                        string conteudo = linha.Trim();
                        
                        // Verificar se a linha é uma tag de botão
                        if (conteudo.Contains("button:"))
                            conteudo = returnLineHtml(conteudo, "button"); 

                            
                        // Verificar se a linha é uma tag de rótulo
                        
                        if (conteudo.Contains("label:"))
                            conteudo = returnLineHtml(conteudo, "label"); 
                        
                        if (conteudo.Contains("text:"))
                            conteudo = returnLineHtml(conteudo, "text"); 

                           
                        if (conteudo.Contains("combobox:"))
                            conteudo = returnLineHtml(conteudo, "combobox"); 

                        
                        if (conteudo.Contains("radio:"))
                            conteudo = returnLineHtml(conteudo, "radio"); 
                            
                        
                        if (conteudo.Contains("checkbox:"))
                            conteudo = returnLineHtml(conteudo, "checkbox"); 
                           
                        writer.WriteLine(fixLineSpacement(conteudo));
                        writer.WriteLine("<br>");
                    }

                    // Escrever o fim do documento HTML
                    writer.WriteLine("</form>");
                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                }

                Console.WriteLine("Arquivo HTML gerado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao processar o arquivo: " + ex.Message);
            }

        }
    
    }
}