
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace projeto_nelson
{
    internal class Program
    {
        // Variavel utilizada para posteriormente ser trocada por espaço dentro das tags.
        static string s = "cod:space";

        // Função que gera um name aleatorio, especificamente, para radio buttons para a identificação do grupo que faz parte.
        static string generateNameInput(){
            int length = 10; // Define o comprimento da string aleatória
            string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; // Define o alfabeto de caracteres possíveis
            
            StringBuilder sb = new StringBuilder(); // Instancia obj de uma classe para manipulação de strings
            Random random = new Random(); // Instancia obj de uma classe para geração de numeros aleatorios

            // Looping com base no tamanho definido para a string
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allowedCharacters.Length); // gera o valor do index de algum dos caracteres permitidos
                char randomChar = allowedCharacters[index]; // escolhe o caractere permitido com base no index sorteado
                sb.Append(randomChar); // Concatena o caractere sorteado a string
            }

            return sb.ToString(); // Converte o objeto da classe StringBuilder para string
        }

        // Função que retornara a linha html, com base no conteudo passado e o tipo da tag.
        static string returnLineHtml(string content, string typeTag) {
            string lineHtml = "";

            if(typeTag == "button") // Caso o tipo da tag for button
                lineHtml = Regex.Replace(content, @"button:(\w+)", $@"<input{s}type=""button""{s}value=""$1"">");
            else if(typeTag == "text") // Caso o tipo da tag seja text
                lineHtml = Regex.Replace(content, @"text:(\w+)", $@"$1:{s}<input{s}type=""text"">");
            else { // Caso o tipo da tag do inṕut seja selecionavel (combobox/radio/checkbox)
                
                // Regex para verificar as options após ":"
                string pattern = $@"{typeTag}:\[(.*?)\]";
                
                lineHtml = Regex.Replace(content, pattern, match =>
                {   
                    string optionsStr = match.Groups[1].Value; // Pega as opções dentro de :[options] e retorna uma string.
                    string[] options = optionsStr.Split(','); // Pega a string e transforma em um array separando onde houver ",".
                    
                    string contentTag = ""; // Inicializa conteudo da tag.
                    string randomName = ""; // Inicializa name aleatorio para input, caso seja uma caixa de seleção.

                    if(typeTag == "combobox") // Se o tipo da tag for combobox, o inicio da tag devera ser <select>
                        contentTag = "<select>";
                    else // Se for uma caixa de seleção, chamamos a função que gera o name aleatorio do input pra caixa de seleção
                        randomName = generateNameInput();
                    
                    // Itera array de opções dentro do input
                    foreach (string option in options){
                        if(typeTag == "combobox") // Caso seja um combobox, cada opção é marcada com a tag <option>
                            contentTag += $"<option>{option}</option>";
                        else // Caso seja radio ou checkbox, cada opção é marcada com input com o type sendo typeTag e o com name gerado aleatoriamente
                            contentTag += $@"<input{s}type=""{typeTag}""{s}name=""{randomName}"">{s}{option}";
                    }

                    if(typeTag == "combobox") contentTag += "</select>"; // Fechamento da tag <select>, caso seja um combobox
                    
                    return contentTag; // Retorna o conteudo HTML apos processar as opções da tag.
                });
            }
            
            return lineHtml; // Retorna linha HTML gerada.
        }

        // Função responsavel por corrigir o espaçamento após gerar as tags ler a linha iterada do txt.
        static string fixLineSpacement(string lineHtml) {
            /* 
                Primeiramente troca tudo onde tiver um espaço " " pela entidade responsavel por adicionar
                espaço no HTML, depois substitui o codigo para espaço dentro das tags (variavel s) por
                espaço " ". 
            */

            return lineHtml.Replace(" ", "&nbsp;&nbsp;").Replace(s, " ");
        }

        static void Main(string[] args)
        {
            // Caminho do arquivo de entrada TXT
            string arquivoTxt = "./public/input.txt";

            // Caminho do arquivo de saída HTML
            string arquivoHtml = "./public/output.html";

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
                        string contentLine = linha;
                        
                        // Verifica se a linha contem a instrução para a geração de um botão
                        if (contentLine.Contains("button:"))
                            contentLine = returnLineHtml(contentLine, "button"); // Se contiver chama a função passando o conteudo até aqui e o tipo como "button"
                        
                        // Verifica se a linha contem a instrução para a geração de um input de texto
                        if (contentLine.Contains("text:"))
                            contentLine = returnLineHtml(contentLine, "text"); // Se contiver chama a função passando o conteudo até aqui e o tipo como "text"

                        // Verifica se a linha contem a instrução para a geração de um input select
                        if (contentLine.Contains("combobox:"))
                            contentLine = returnLineHtml(contentLine, "combobox"); // Se contiver chama a função passando o conteudo até aqui e o tipo como "combobox"

                        // Verifica se a linha contem a instrução para a geração de um input radio
                        if (contentLine.Contains("radio:"))
                            contentLine = returnLineHtml(contentLine, "radio"); // Se contiver chama a função passando o conteudo até aqui e o tipo como "radio"
                            
                        // Verifica se a linha contem a instrução para a geração de um input checkbox
                        if (contentLine.Contains("checkbox:"))
                            contentLine = returnLineHtml(contentLine, "checkbox"); // Se contiver chama a função passando o conteudo até aqui e o tipo como "checkbox"
                           
                        writer.WriteLine(fixLineSpacement(contentLine)); // Chama função para corrigir espaços e escreve a linha no html
                        
                        writer.WriteLine("<br>"); // Pula uma linha no html
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