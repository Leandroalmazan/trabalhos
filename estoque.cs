using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleEstoque
{
    // CLASSE PRODUTO
    public class Produto
    {
        // Atributos do Produto
        public string Nome { get; private set; }
        public double Preco { get; private set; }
        public int Quantidade { get; private set; }

        // Construtor para inicializar um novo produto
        public Produto(string nome, double preco, int quantidade)
        {
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
        }

        // Método para incrementar a quantidade em estoque
        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade > 0)
            {
                Quantidade += quantidade;
                Console.WriteLine($"\n{quantidade} unidade(s) do produto '{Nome}' adicionada(s) ao estoque. Nova quantidade: {Quantidade}");
            }
            else
            {
                Console.WriteLine("\nA quantidade a ser adicionada deve ser positiva.");
            }
        }

        // Método para decrementar a quantidade em estoque
        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
            {
                Console.WriteLine("\nA quantidade a ser removida deve ser positiva.");
                return;
            }

            if (Quantidade >= quantidade)
            {
                Quantidade -= quantidade;
                Console.WriteLine($"\n{quantidade} unidade(s) do produto '{Nome}' removida(s) do estoque. Nova quantidade: {Quantidade}");
            }
            else
            {
                Console.WriteLine($"\nNão há estoque suficiente de '{Nome}' para remover {quantidade} unidade(s). Quantidade atual: {Quantidade}");
            }
        }

        // Sobrescrevendo o método ToString para exibir os detalhes do produto
        public override string ToString()
        {
            return $"Nome: {Nome}, Preço: R$ {Preco:F2}, Quantidade em Estoque: {Quantidade}";
        }
    }

    // CLASSE PRINCIPAL DO PROGRAMA
    class Program
    {
        // Lista para armazenar os produtos em memória
        static List<Produto> estoque = new List<Produto>();

        static void Main(string[] args)
        {
            int opcao = 0;

            do
            {
                Console.WriteLine("\n--- SISTEMA DE CONTROLE DE ESTOQUE ---");
                Console.WriteLine("1. Cadastrar novo produto");
                Console.WriteLine("2. Incrementar estoque de um produto");
                Console.WriteLine("3. Decrementar estoque de um produto");
                Console.WriteLine("4. Listar todos os produtos");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");

                // Usando TryParse para entrada do menu principal (Mais seguro que int.Parse)
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("\nErro: Entrada inválida. Por favor, insira um número de 1 a 5.");
                    opcao = 0; // Reseta a opção para continuar no loop
                    continue;
                }

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            CadastrarProduto();
                            break;
                        case 2:
                            IncrementarEstoque();
                            break;
                        case 3:
                            DecrementarEstoque();
                            break;
                        case 4:
                            ListarProdutos();
                            break;
                        case 5:
                            Console.WriteLine("\nSaindo do sistema...");
                            break;
                        default:
                            Console.WriteLine("\nOpção inválida! Tente novamente.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Captura quaisquer outras exceções não tratadas (ex: falha de I/O)
                    Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
                    opcao = 0;
                }

            } while (opcao != 5);
        }

        public static void CadastrarProduto()
        {
            Console.WriteLine("\n--- Cadastro de Novo Produto ---");

            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine();

            // Verifica se o produto já existe (ignorando maiúsculas/minúsculas)
            if (estoque.Any(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("\nErro: Um produto com este nome já foi cadastrado.");
                return;
            }

            // Usa a função de validação pública
            double preco = LerDoubleValido("Preço do produto: ");
            // Usa a função de validação pública
            int quantidade = LerIntNaoNegativoValido("Quantidade inicial em estoque: ");

            Produto novoProduto = new Produto(nome, preco, quantidade);
            estoque.Add(novoProduto);

            Console.WriteLine("\nProduto cadastrado com sucesso!");
        }

        public static Produto BuscarProdutoPorNome()
        {
            Console.Write("\nDigite o nome do produto: ");
            string nomeBusca = Console.ReadLine();

            Produto produto = estoque.FirstOrDefault(p => p.Nome.Equals(nomeBusca, StringComparison.OrdinalIgnoreCase));

            if (produto == null)
            {
                Console.WriteLine("\nProduto não encontrado.");
            }

            return produto;
        }

        public static void IncrementarEstoque()
        {
            Console.WriteLine("\n--- Adicionar ao Estoque ---");
            Produto produto = BuscarProdutoPorNome();

            if (produto != null)
            {
                // Usa a função de validação pública
                int quantidade = LerIntPositivoValido($"Digite a quantidade a ser adicionada ao estoque de '{produto.Nome}': ");
                produto.AdicionarEstoque(quantidade);
            }
        }

        public static void DecrementarEstoque()
        {
            Console.WriteLine("\n--- Remover do Estoque ---");
            Produto produto = BuscarProdutoPorNome();

            if (produto != null)
            {
                // Usa a função de validação pública
                int quantidade = LerIntPositivoValido($"Digite a quantidade a ser removida do estoque de '{produto.Nome}': ");
                produto.RemoverEstoque(quantidade);
            }
        }

        public static void ListarProdutos()
        {
            Console.WriteLine("\n--- Lista de Produtos em Estoque ---");
            if (estoque.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado no momento.");
            }
            else
            {
                foreach (var produto in estoque)
                {
                    Console.WriteLine(produto.ToString());
                }
            }
        }

        // --- FUNÇÕES DE VALIDAÇÃO DE ENTRADA (AGORA PUBLICAS PARA RESOLVER O ERRO DE ACESSO) ---

        // Validação para Preço (double não negativo)
        public static double LerDoubleValido(string mensagem)
        {
            double valor = -1;
            bool valido = false;
            while (!valido)
            {
                Console.Write(mensagem);
                if (double.TryParse(Console.ReadLine(), out valor) && valor >= 0)
                {
                    valido = true;
                }
                else
                {
                    Console.WriteLine("Erro: Entrada inválida. Por favor, digite um valor numérico não negativo (ex: 10.50).");
                }
            }
            return valor;
        }

        // Validação para Quantidade Inicial (int não negativo: pode ser 0 ou mais)
        public static int LerIntNaoNegativoValido(string mensagem)
        {
            int valor = -1;
            bool valido = false;
            while (!valido)
            {
                Console.Write(mensagem);
                if (int.TryParse(Console.ReadLine(), out valor) && valor >= 0)
                {
                    valido = true;
                }
                else
                {
                    Console.WriteLine("Erro: Entrada inválida. Por favor, digite um número inteiro não negativo (0, 1, 2...).");
                }
            }
            return valor;
        }

        // Validação para Adicionar/Remover Estoque (int positivo: deve ser maior que 0)
        public static int LerIntPositivoValido(string mensagem)
        {
            int valor = -1;
            bool valido = false;
            while (!valido)
            {
                Console.Write(mensagem);
                if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
                {
                    valido = true;
                }
                else
                {
                    Console.WriteLine("Erro: Entrada inválida. Por favor, digite um número inteiro positivo (maior que zero).");
                }
            }
            return valor;
        }
    }
}