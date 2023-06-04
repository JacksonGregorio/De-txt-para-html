
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace projeto_nelson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string arquivoTxt = "C:\\Users\\User\\Desktop\\txtteste\\hatmlteste.txt";

            // Caminho do arquivo de saída HTML
            string arquivoHtml = "C:\\Users\\User\\Desktop\\txtteste\\index43.html";

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

                    // Iterar sobre as linhas do arquivo TXT
                    foreach (string linha in linhas)
                    {
                        // Remover espaços em branco no início e no fim da linha
                        string conteudo = linha.Trim();

                        // Verificar se a linha é uma tag de botão
                        if (conteudo.StartsWith("button:"))
                        {
                            // Extrair o nome do botão
                            string nomeBotao = conteudo.Substring(7);

                            // Gerar a tag de botão no HTML
                            writer.WriteLine("<button type=\"button\">" + nomeBotao + "</button>");
                        }
                        // Verificar se a linha é uma tag de rótulo
                        else if (conteudo.StartsWith("label:"))
                        {
                            // Extrair o nome do rótulo
                            string nomeRotulo = conteudo.Substring(6);

                            // Gerar a tag de rótulo no HTML
                            writer.WriteLine("<label>" + nomeRotulo + "</label>");
                        }
                        // Verificar se a linha contém caracteres "_" ou "|"
                        else if (conteudo.Contains("_") || conteudo.Contains("|"))
                        {
                            // Gerar a tag de parágrafo no HTML com os caracteres especiais
                            writer.WriteLine("<p>" + conteudo + "</p>");
                        }
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
