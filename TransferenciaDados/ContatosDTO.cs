﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//referencias para acesso ao banco de dados
using MySql.Data.MySqlClient;
using System.Data;
using AcessoBanco;


namespace TransferenciaDados
{
    public class ContatosDTO
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public int numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string mensagens { get; set; }
    }

    public class SalvarContatos
    {
        public void ContatosIncuir(ContatosDTO dados)
        {
            try
            {
                //Definir o tipo de comandos do SQL
                MySqlCommand cmd = new MySqlCommand("spInserirContatos", Conexao.obterConexao());
                cmd.CommandType = CommandType.StoredProcedure;

                //Popular os parametros de procedure
                cmd.Parameters.AddWithValue("@pnome", dados.nome);
                cmd.Parameters.AddWithValue("@pendereco", dados.endereco + "," + dados.numero);
                cmd.Parameters.AddWithValue("@pbairro", dados.bairro);
                cmd.Parameters.AddWithValue("@pcidade", dados.cidade);
                cmd.Parameters.AddWithValue("@puf", dados.uf);
                cmd.Parameters.AddWithValue("@pcep", dados.cep);
                cmd.Parameters.AddWithValue("@ptelefone", dados.telefone);
                cmd.Parameters.AddWithValue("@pemail", dados.email);

                //Executar os comandos SQL
                MySqlDataReader dr = cmd.ExecuteReader();

                //Verificar a existência de registros
                if (dr.HasRows)
                {
                    //Percorre os registros
                    while (dr.Read())
                    {
                        dados.codigo = Convert.ToInt32(dr.GetValue(0).ToString());
                    }
                }
                dr.Close();
                Conexao.fecharConexao();
            }
            catch (MySqlException e)
            {
                dados.mensagens = "Erro - SalvarContatos - Contatos Incluir - " + e.Message.ToString();
            }
        }
    }
    public class ConsultarContatos
    {
        public DataTable ContatosDataTable;

        public void ContatosMostrar (ContatosDTO dados)
        {
            try
            {
                string sql = "Select * from tbContatos order by nomedoContato asc";
                //Interação de dados
                MySqlDataAdapter ContatosDataAdapter = new MySqlDataAdapter(sql, Conexao.obterConexao());
                //Instanciar o objeto
                ContatosDataTable = new DataTable();
                //Popular o DataTable
                ContatosDataAdapter.Fill(ContatosDataTable);

                Conexao.fecharConexao();
            }
            catch (MySqlException e)
            {
                dados.mensagens = "Erro - ConsultarContatos - MostrarContatos " + e.Message.ToString();
            }
        }
    }
}
