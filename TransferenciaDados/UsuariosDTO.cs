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
    public class UsuariosDTO
    {
        public string nomeusuario { get; set; }
        public string senhausuario { get; set; }
        public string emailusuario { get; set; }
        public int logado { get; set; }
        public string nomecompleto { get; set; }
        public string mensagens { get; set; }
        public string novasenha { get; set; }
        public int codperfil { get; set; }
        public byte[] foto { get; set; }
    }
    public class LoginSistema
    {
        public static string usuario { get; set; }
        public static string nomecompleto { get; set; }
        public static int codperfil { get; set; }
    }
    public class ValidarUsuario
    {
        public void UsuariosConectar(UsuariosDTO dados)
        {
            try
            {
                //Definir comandos para execução
                MySqlCommand cmd = new MySqlCommand("spValidarUsuarios", Conexao.obterConexao());
                // Definir como stored Procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //parâmetros de entrada
                cmd.Parameters.AddWithValue("@pnomeusuario", dados.nomeusuario);
                cmd.Parameters.AddWithValue("@psenhausuario", dados.senhausuario);

                //Executar os comandos sql
                MySqlDataReader dr = cmd.ExecuteReader();

                //verificar existencia de registros
                if (dr.HasRows)
                {
                    //percorre os registros
                    while (dr.Read())
                    {
                        //Verifiacar o retorno do parâmetro resultado
                        int resultado = Convert.ToInt32(dr.GetValue(0).ToString());
                        if (resultado > 0)
                        {
                            //popular o resultado
                            LoginSistema.usuario = dados.nomeusuario;
                            LoginSistema.nomecompleto = dr.GetValue(2).ToString();
                            dados.logado = Convert.ToInt32(dr.GetValue(1).ToString());
                        }
                        else
                        {
                            dados.logado = resultado;
                        }
                    }
                }
                dr.Close();
                Conexao.fecharConexao();
            }
            catch (MySqlException e)
            {
                dados.mensagens = "Erro - ValidarUsuarios - UsuariosConectar - " + e.Message.ToString();
            }
        }
        public void UsuariosDesconectar(UsuariosDTO dados)
        {
            try
            {
                //Definir instruções SQL
                string sql = "update tbusuarios " +
                    "set logado = 1" +
                    "where nomeusuario = @usuario";
                //definir comando para execução
                MySqlCommand cmd = new MySqlCommand(sql, Conexao.obterConexao());
                //comando tipo texto
                cmd.CommandType = CommandType.Text;
                //popular o parametro
                cmd.Parameters.Add(new MySqlParameter("@usuario", dados.nomeusuario));
                //executar os comandos
                int registros = cmd.ExecuteNonQuery();

                if (registros >= 1)
                {
                    dados.mensagens = registros.ToString();
                }
                Conexao.fecharConexao();
            }
            catch (MySqlException e)
            {
                dados.mensagens = "Erro - ValidarUsuarios - UsuariosDesconectar - " + e.Message.ToString();
            }
        }
    }
}
