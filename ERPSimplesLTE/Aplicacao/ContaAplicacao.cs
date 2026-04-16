using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPSimplesLTE.Models;
using ERPSimplesLTE.Repositorio;
using ERPSimplesLTE.Aplicacao;

namespace ERPSimplesLTE.Aplicacao
{
    public class ContaAplicacao
    {
        private readonly Contexto contexto;

        public ContaAplicacao()
        {
            contexto = new Contexto();
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuarios = new List<Usuario>();
            const string strQuery = "SELECT Email, Senha FROM Usuarios WHERE Email = @email AND Senha = @senha";

            var parametros = new Dictionary<string, object>
                {
                    {"Email", email},
                    {"Senha", senha}
                };

            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario
                {
                    Email = row["Email"]
                };
                usuarios.Add(tempUsuario);
            }
            
            return usuarios.FirstOrDefault();
        }

        public int Inserir(Usuario usuario)
        {
            const string commandText = " INSERT INTO Usuarios (Nome, Email, Login, Celular, Senha, SenhaConfirmar, Supervisor, Situacao, DataCadastro, Observacao) VALUES (@Nome, @Email, @Login, @Celular, @Senha, @SenhaConfirmar, @Supervisor, @Situacao, @DataCadastro, @Observacao) ";

            var parameters = new Dictionary<string, object>
            {
                {"Nome", usuario.Nome},
                {"Email", usuario.Email},
                {"Login", usuario.Login},
                {"Celular", usuario.Celular},
                {"Senha", usuario.Senha},
                {"SenhaConfirmar", usuario.SenhaConfirmar},
                {"Supervisor", usuario.Supervisor},
                {"Situacao", usuario.Situacao.Id},
                {"DataCadastro", usuario.DataCadastro},
                {"Observacao", usuario.Observacao}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Usuario RecupérarSenha(string email)
        {
            var usuarios = new List<Usuario>();
            const string strQuery = "SELECT Email, Senha, SenhaConfirmar FROM Usuarios WHERE Email = @email";

            var parametros = new Dictionary<string, object>
                {
                    {"Email", email}
                };

            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario
                {
                    Email = row["Email"],
                    Senha = row["Senha"],
                    SenhaConfirmar = row["SenhaConfirmar"]
                };
                usuarios.Add(tempUsuario);
            }

            return usuarios.FirstOrDefault();
        }

        public int AlterarSenha(string email, string senha, string senhaConfirmar)
        {

            var commandText = " UPDATE Usuarios SET ";
            commandText    += " Senha = '" + senha + "',";
            commandText    += " SenhaConfirmar = '" + senhaConfirmar + "'";
            commandText    += " WHERE Email = '" + email + "'";

            var parameters = new Dictionary<string, object>
            {
                {"Email", email},
                {"Senha", senha},
                {"SenhaConfirmar", senhaConfirmar}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public Usuario ValidarEmail(string email)
        {
            var usuarios = new List<Usuario>();
            const string strQuery = "SELECT Email FROM Usuarios WHERE Email = @email";

            var parametros = new Dictionary<string, object>
                {
                    {"Email", email}
                };

            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario
                {
                    Email = row["Email"]
                };
                usuarios.Add(tempUsuario);
            }

            return usuarios.FirstOrDefault();
        }
        public Usuario ConfirmarSenha(string email, string senha, string senhaconfirmar)
        {
            var usuarios = new List<Usuario>();
            const string strQuery = "SELECT Senha, SenhaConfirmar FROM Usuarios WHERE Email = @email";

            var parametros = new Dictionary<string, object>
                {
                    {"Email", email},
                    {"Senha", senha},
                    {"SenhaConfirmar", senhaconfirmar}
                };

            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario
                {
                    Senha = row["Senha"],
                    SenhaConfirmar = row["SenhaConfirmar"]
                };
                usuarios.Add(tempUsuario);
            }

            return usuarios.FirstOrDefault();
        }

        public List<Usuario> ConsultaRegistros()
        {
            var usuarios = new List<Usuario>();
            var strQuery = "SELECT Usuarios.Id as UsId, Usuarios.Login as UsLogin, Usuarios.Senha as UsSenha, Usuarios.SenhaConfirmar as UsSenhaConfirmar, ";
            strQuery += " Usuarios.Nome as UsNome, Usuarios.Email as UsEmail, Usuarios.Celular as UsCelular, Usuarios.Supervisor as UsSupervisor, Usuarios.Situacao as UsSituacao, ";
            strQuery += " Usuarios.DataCadastro as UsDataCadastro, Usuarios.DataAlteracao as UsDataAlteracao, Usuarios.UltimoLogin as UsUltimoLogin, Usuarios.Observacao as UsObservacao, ";
            strQuery += " Situacao.Id as SiId, Situacao.Valor as SiValor, Situacao.Texto as SiTexto, Situacao.Parametro as SiParametro ";
            strQuery += " FROM Usuarios ";
            strQuery += " INNER JOIN Situacao on Situacao.Id = Usuarios.Situacao ";
            strQuery += " Order by Usuarios.Id Asc  ";

            var rows = contexto.ExecutaComandoComRetorno(strQuery, null);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario()
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["UsId"]) ? row["UsId"] : "0"),
                    Login = row["UsLogin"],
                    Senha = row["UsSenha"],
                    SenhaConfirmar = row["UsSenhaConfirmar"],
                    Nome = row["UsNome"],
                    Email = row["UsEmail"],
                    Celular = row["UsCelular"],
                    Supervisor = bool.Parse(!string.IsNullOrEmpty(row["UsSupervisor"]) ? row["UsSupervisor"] : "0"),
                    Situacao = {
                                    Id = int.Parse(!string.IsNullOrEmpty(row["SiId"]) ? row["SiId"] : "0"),
                                    Valor = int.Parse(!string.IsNullOrEmpty(row["SiValor"]) ? row["SiValor"] : "0"),
                                    Texto = row["SiTexto"],
                                    Parametro = row["SiParametro"],
                                },
                    DataCadastro = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataCadastro"]) ? row["UsDataCadastro"] : "1000-01-01 00:00:00"),
                    DataAlteracao = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataAlteracao"]) ? row["UsDataAlteracao"] : "1000-01-01 00:00:00"),
                    UltimoLogin = DateTime.Parse(!string.IsNullOrEmpty(row["UsUltimoLogin"]) ? row["UsUltimoLogin"] : "1000-01-01 00:00:00"),
                    Observacao = row["UsObservacao"]
                };
                usuarios.Add(tempUsuario);
            }

            return usuarios;
        }

        public Usuario ConsultaPorId(int id)
        {
            var usuarios = new List<Usuario>();
            var strQuery = "SELECT Usuarios.Id as UsId, Usuarios.Login as UsLogin, Usuarios.Senha as UsSenha, Usuarios.SenhaConfirmar as UsSenhaConfirmar, ";
            strQuery += " Usuarios.Nome as UsNome, Usuarios.Email as UsEmail, Usuarios.Celular as UsCelular, Usuarios.Supervisor as UsSupervisor, Usuarios.Situacao as UsSituacao, ";
            strQuery += " Usuarios.DataCadastro as UsDataCadastro, Usuarios.DataAlteracao as UsDataAlteracao, Usuarios.UltimoLogin as UsUltimoLogin, Usuarios.Observacao as UsObservacao, ";
            strQuery += " Situacao.Id as SiId, Situacao.Valor as SiValor, Situacao.Texto as SiTexto, Situacao.Parametro as SiParametro ";
            strQuery += " FROM Usuarios ";
            strQuery += " INNER JOIN Situacao on Situacao.Id = Usuarios.Situacao ";
            strQuery += " WHERE Usuarios.Id = " + id ;

            var parametros = new Dictionary<string, object>
            {
                {"Usuarios.Id", id}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario()
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["UsId"]) ? row["UsId"] : "0"),
                    Login = row["UsLogin"],
                    Senha = row["UsSenha"],
                    SenhaConfirmar = row["UsSenhaConfirmar"],
                    Nome = row["UsNome"],
                    Email = row["UsEmail"],
                    Celular = row["UsCelular"],
                    Supervisor = bool.Parse(!string.IsNullOrEmpty(row["UsSupervisor"]) ? row["UsSupervisor"] : "0"),
                    Situacao = {
                                    Id = int.Parse(!string.IsNullOrEmpty(row["SiId"]) ? row["SiId"] : "0"),
                                    Valor = int.Parse(!string.IsNullOrEmpty(row["SiValor"]) ? row["SiValor"] : "0"),
                                    Texto = row["SiTexto"],
                                    Parametro = row["SiParametro"],
                                },
                    DataCadastro = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataCadastro"]) ? row["UsDataCadastro"] : "1000-01-01 00:00:00"),
                    DataAlteracao = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataAlteracao"]) ? row["UsDataAlteracao"] : "1000-01-01 00:00:00"),
                    UltimoLogin = DateTime.Parse(!string.IsNullOrEmpty(row["UsUltimoLogin"]) ? row["UsUltimoLogin"] : "1000-01-01 00:00:00"),
                    Observacao = row["UsObservacao"]
                };
                usuarios.Add(tempUsuario);
            }
            return usuarios.FirstOrDefault();
        }
        public int Editar(Usuario usuario)
        {

            var commandText = " UPDATE Usuarios SET ";
            commandText += " Nome = @Nome, ";
            commandText += " Email = @Email, ";
            commandText += " Login = @Login, ";
            commandText += " Celular = @Celular, ";
            commandText += " Senha = @Senha, ";
            commandText += " SenhaConfirmar = @SenhaConfirmar, ";
            commandText += " Supervisor = @Supervisor, ";
            commandText += " Situacao = @Situacao, ";
            commandText += " DataAlteracao = @DataAlteracao, ";
            commandText += " Observacao = @Observacao ";
            commandText += " WHERE Id = @Id";

            var parameters = new Dictionary<string, object>
            {
                {"Id", usuario.Id},
                {"Nome", usuario.Nome},
                {"Email", usuario.Email},
                {"Login", usuario.Login},
                {"Celular", usuario.Celular},
                {"Senha", usuario.Senha},
                {"SenhaConfirmar", usuario.SenhaConfirmar},
                {"Supervisor", usuario.Supervisor},
                {"Situacao", usuario.Situacao.Id},
                {"DataAlteracao", usuario.DataAlteracao},
                {"Observacao", usuario.Observacao}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }

        public void Salvar(Usuario usuario)
        {
            if (usuario.Id > 0)
                Editar(usuario);
            else
                Inserir(usuario);
        }

        public int Excluir(int id)
        {
            const string strQuery = "DELETE FROM Usuarios WHERE Id = @Id";
            var parametros = new Dictionary<string, object>
            {
                {"Id", id}
            };
            return contexto.ExecutaComando(strQuery, parametros);
        }
        public List<Situacao> ConsultaSituacaoPorParametro(string parametro)
        {
            var situacao = new List<Situacao>();
            var strQuery = "SELECT Valor, Texto From Situacao WHERE Parametro = @parametro   ";

            var parametros = new Dictionary<string, object>
                {
                    {"Parametro", parametro}
                };

            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempSituacao = new Situacao
                {
                    Valor = int.Parse(!string.IsNullOrEmpty(row["Valor"]) ? row["Valor"] : "0"),
                    Texto = row["Texto"]
                };
                situacao.Add(tempSituacao);
            }

            return situacao;
        }
        public Usuario ConsultaPorEmail(string email)
        {
            var usuarios = new List<Usuario>();
            var strQuery = "SELECT Usuarios.Id as UsId, Usuarios.Login as UsLogin, Usuarios.Senha as UsSenha, Usuarios.SenhaConfirmar as UsSenhaConfirmar, ";
            strQuery += " Usuarios.Nome as UsNome, Usuarios.Email as UsEmail, Usuarios.Celular as UsCelular, Usuarios.Supervisor as UsSupervisor, Usuarios.Situacao as UsSituacao, ";
            strQuery += " Usuarios.DataCadastro as UsDataCadastro, Usuarios.DataAlteracao as UsDataAlteracao, Usuarios.UltimoLogin as UsUltimoLogin, Usuarios.Observacao as UsObservacao, ";
            strQuery += " Situacao.Id as SiId, Situacao.Valor as SiValor, Situacao.Texto as SiTexto, Situacao.Parametro as SiParametro ";
            strQuery += " FROM Usuarios ";
            strQuery += " INNER JOIN Situacao on Situacao.Id = Usuarios.Situacao ";
            strQuery += " WHERE Usuarios.Email = '" + email + "'";

            var parametros = new Dictionary<string, object>
            {
                {"Usuarios.Email", email}
            };
            var rows = contexto.ExecutaComandoComRetorno(strQuery, parametros);
            foreach (var row in rows)
            {
                var tempUsuario = new Usuario()
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["UsId"]) ? row["UsId"] : "0"),
                    Login = row["UsLogin"],
                    Senha = row["UsSenha"],
                    SenhaConfirmar = row["UsSenhaConfirmar"],
                    Nome = row["UsNome"],
                    Email = row["UsEmail"],
                    Celular = row["UsCelular"],
                    Supervisor = bool.Parse(!string.IsNullOrEmpty(row["UsSupervisor"]) ? row["UsSupervisor"] : "0"),
                    Situacao = {
                                    Id = int.Parse(!string.IsNullOrEmpty(row["SiId"]) ? row["SiId"] : "0"),
                                    Valor = int.Parse(!string.IsNullOrEmpty(row["SiValor"]) ? row["SiValor"] : "0"),
                                    Texto = row["SiTexto"],
                                    Parametro = row["SiParametro"],
                                },
                    DataCadastro = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataCadastro"]) ? row["UsDataCadastro"] : "1000-01-01 00:00:00"),
                    DataAlteracao = DateTime.Parse(!string.IsNullOrEmpty(row["UsDataAlteracao"]) ? row["UsDataAlteracao"] : "1000-01-01 00:00:00"),
                    UltimoLogin = DateTime.Parse(!string.IsNullOrEmpty(row["UsUltimoLogin"]) ? row["UsUltimoLogin"] : "1000-01-01 00:00:00"),
                    Observacao = row["UsObservacao"]
                };
                usuarios.Add(tempUsuario);
            }
            return usuarios.FirstOrDefault();
        }
        public int UltimoAcesso(string email, string ultimologin)
        {
            var commandText = " UPDATE Usuarios SET ";
            commandText += " UltimoLogin = '" + ultimologin + "'";
            commandText += " WHERE Email = '" + email + "'";

            var parameters = new Dictionary<string, object>
            {
                {"Email", email},
                {"UltimoLogin", ultimologin}
            };

            return contexto.ExecutaComando(commandText, parameters);
        }
    }
}