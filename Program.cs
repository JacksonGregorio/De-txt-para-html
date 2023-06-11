
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace projeto_nelson
{
    internal class Program
    {
        static string returnLineHtml(string content, string typeTag) {
            string startTag = "";
            string endTag = "";

            if(typeTag == "button"){
                startTag = "<button>";
                endTag = "</button>";
            } else if(typeTag == "label"){
                startTag = "<label>";
                endTag = "</label>";
            } else {
                startTag = "<p>";
                endTag = "</p>";
            }

            string linhaHTML = System.Text.RegularExpressions.Regex.Replace(content, $@"{typeTag}:(.*)", match =>
                {
                    string contentTag = match.Groups[1].Value.Trim();
                    return startTag + contentTag + endTag;
                });

            return linhaHTML;
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
                 writer.WriteLine("<style>h1 {text-align: center; font-family: Arial, sans-serif; font-size: 48px; color: #FF0000; text-shadow: 2px 2px 4px #000000;} @keyframes fireworks-animation {0% {color: #FF0000; text-shadow: 2px 2px 4px #000000;} 14.285% {color: #FF8000; text-shadow: 2px 2px 4px #000000;} 28.57% {color: #FFFF00; text-shadow: 2px 2px 4px #000000;} 42.855% {color: #00FF00; text-shadow: 2px 2px 4px #000000;} 57.14% {color: #0000FF; text-shadow: 2px 2px 4px #000000;} 71.425% {color: #8000FF; text-shadow: 2px 2px 4px #000000;} 85.71% {color: #FF00FF; text-shadow: 2px 2px 4px #000000;} 100% {color: #FF0080; text-shadow: 2px 2px 4px #000000;}} h1.fireworks-animation {animation: fireworks-animation 2s infinite;}</style>");
                 writer.WriteLine("</head>");
                 writer.WriteLine("<body>");

                    // Iterar sobre as linhas do arquivo TXT
                    foreach (string linha in linhas)
                    {
                        // Remover espaços em branco no início e no fim da linha
                        string conteudo = linha.Trim();

                        // Verificar se a linha é uma tag de botão
                        if (conteudo.Contains("button:"))
                        {
                            writer.WriteLine(returnLineHtml(conteudo, "button"));
                        }
                        // Verificar se a linha é uma tag de rótulo
                        else if (conteudo.Contains("label:"))
                        {
                           writer.WriteLine(returnLineHtml(conteudo, "label"));
                        }
                        // Verificar se a linha contém caracteres "_" ou "|"
                        else if (conteudo.Contains("_") || conteudo.Contains("|"))
                        {
                            // Gerar a tag de parágrafo no HTML com os caracteres especiais
                            writer.WriteLine("<p>" + conteudo + "</p>");
                        }

                        writer.WriteLine("  <br>");
                    }

                    // Escrever o fim do documento HTML
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
