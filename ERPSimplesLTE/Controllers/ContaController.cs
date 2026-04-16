using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSimplesLTE.Aplicacao;
using ERPSimplesLTE.Models;

namespace ERPSimplesLTE.Controllers
{
    public class ContaController : Controller
    {
        private readonly ContaAplicacao contaAplicacao;
        public ContaController()
        {
            contaAplicacao = new ContaAplicacao();
        }
        public ActionResult Index()
        {
            return RedirectToAction("Login","Conta");
        }

        public ActionResult Login(string email, string senha)
        {
            TempData["LoginInvalido"] = "";
            if (email != null && senha != null)
            {
                var objveremail= contaAplicacao.ValidarEmail(email);
                if (objveremail != null)
                {
                    var objusuario = contaAplicacao.ValidarLogin(email, senha);
                    if (objusuario != null)
                    {
                        var objsituacao = contaAplicacao.ConsultaPorEmail(email);

                        bool verificasituacao = objsituacao.Situacao.Id.Value == 2 || objsituacao.Situacao.Id.Value == 3 ;
                        if (verificasituacao == true)
                        {
                            DateTime ultimologin = DateTime.Now;
                            string sdtultimologin = ultimologin.ToString("yyyy-MM-dd H:mm:ss");

                            contaAplicacao.UltimoAcesso(email, sdtultimologin);
                            return RedirectToAction("Inicio", "Home");
                        }
                        else
                        {
                            TempData["LoginInvalido"] = "O Acesso não está liberado, favor verificar o motivo";
                        }
                    }
                    else
                    {
                        TempData["LoginInvalido"] = "A Senha digitada está incorreta";
                    }
                }
                else
                {
                    TempData["LoginInvalido"] = "Email não cadastrado";
                }
            }
            return View();
        }

        public ActionResult Registrar(string nome, string email, string senha, string senhaconfirmar, Nullable<int> situacao)
        {

            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.Nome = nome;
                usuario.Email = email;
                usuario.Senha = senha;
                usuario.SenhaConfirmar = senhaconfirmar;
                usuario.Situacao.Id = situacao;

                string Msg = "";
                try { if (string.IsNullOrWhiteSpace(usuario.Nome))  Msg += "Nome invalido"; }  catch (Exception) { usuario.Nome = ""; }
                try { if (string.IsNullOrWhiteSpace(usuario.Email)) Msg += "Email invalido"; } catch (Exception) { usuario.Email = ""; }
                try { if (string.IsNullOrWhiteSpace(usuario.Senha)) Msg += "Senha invalida"; } catch (Exception) { usuario.Senha = ""; }
                try { if (string.IsNullOrWhiteSpace(usuario.SenhaConfirmar)) Msg += "Senha Confirmar invalida"; } catch (Exception) { usuario.SenhaConfirmar = ""; }
                try { if (usuario.Situacao.Id == 0) Msg += "Situação invalida"; } catch (Exception) { usuario.Situacao.Id = 0; }

                if (Msg == "") 
                {
                    string existemail = EmailJaExiste(usuario.Email);

                    if (!string.IsNullOrWhiteSpace(existemail) && (existemail != null) && (existemail != "")) { ViewBag.RetornoEmailJaCadastrado = existemail; } else { }

                    string comparasenha = ComparaSenhas(usuario.Email, usuario.Senha, usuario.SenhaConfirmar);

                    if (!string.IsNullOrWhiteSpace(comparasenha) && (comparasenha != null) && (comparasenha != "")) { ViewBag.RetornoComparaSenha = comparasenha; } else { }

                    if (existemail == "" && comparasenha == "")
                    {
                        contaAplicacao.Inserir(usuario);
                        return RedirectToAction("Inicio", "Home");
                    }
                }
            }
            return View();
        }

        public ActionResult RecuperarSenha(Usuario usuario)
        {
            TempData["cEmailInvalido"] = "";
            if (usuario.Email != null )
            {
                var objusuario = contaAplicacao.ValidarEmail(usuario.Email);
                if (objusuario == null)
                {
                    TempData["cEmailInvalido"] = "Email não cadastrado";
                }
                else
                {
                    System.Web.HttpContext.Current.Session["cEmail"] = objusuario.Email;
                    return RedirectToAction("InformarSenha", "Conta");
                }
            }
            return View();
        }

        public ActionResult InformarSenha(string senha, string senhaconfirmar)
        {

            string sEmail = System.Web.HttpContext.Current.Session["cEmail"].ToString();
            string Msg = "";
            TempData["cSenha"] = "";
            var objusuario = contaAplicacao.RecupérarSenha(sEmail);
            if (objusuario != null)
            {
                try { if (string.IsNullOrWhiteSpace(senha)) Msg += "Senha invalida"; } catch (Exception) { Msg = ""; }
                try { if (string.IsNullOrWhiteSpace(senhaconfirmar)) Msg += "Senha Confirmar invalida"; } catch (Exception) { Msg = ""; }
                if (Msg == "")
                {

                    if (senha == senhaconfirmar)
                    {
                        contaAplicacao.AlterarSenha(sEmail, senha, senhaconfirmar);
                        return RedirectToAction("Login", "Conta");
                    }
                    else
                    {
                        TempData["cSenha"] = "Senhas são diferentes!\n Digite Novamente.";
                    }
                }
            }
            return View();
        }

        public string EmailNaoCadastrado(string email)
        {
           string Msg = "";

            if (email != null)
            {
                var stringemail = contaAplicacao.ValidarEmail(email);
                if (stringemail == null) { Msg = "Email não Cadastrado"; } else { Msg = ""; }
            }
            return Msg;
        }

        public string EmailJaExiste(string email)
        {
            string Msg = "";
            if (email != null)
            {
                try { if (!string.IsNullOrWhiteSpace(email)) Msg += "Email ja cadastrado"; } catch (Exception) { email = ""; }

                var stringemail = contaAplicacao.ValidarEmail(email);
                if (stringemail == null)
                    return "";
            }
            return Msg;
        }

        public string ComparaSenhas(string email, string senha, string senhaconfirmar)
        {
            string Msg = "";
            if (senha != null && senhaconfirmar != null)
            {
                if (senha == senhaconfirmar )
                {
                    var stringsenha = contaAplicacao.ConfirmarSenha(email, senha, senhaconfirmar);
                    if (stringsenha == null)
                        return "";
                }
                else
                {
                   Msg += "Senhas são diferentes!\n Digite Novamente.";
                }
            }
            return Msg;
        }

        public ActionResult Busca()
        {
            var lista = contaAplicacao.ConsultaRegistros();
            
            return View(lista);
        }

        public ActionResult Formulario(int id)
        {
            var carregarSituacao = ConsultaSituacao("SituacaoPagamento");

            Usuario classeinserir = new Usuario();
            classeinserir.Situacao.Situacoes = carregarSituacao;

            Usuario classealterar = new Usuario();

            if (id > 0)
            {
                var usuario = contaAplicacao.ConsultaPorId(id);
                classealterar = usuario;
                classealterar.Situacao.Situacoes = carregarSituacao;

                var selectedItem = usuario.Situacao.Situacoes.Find(p => p.Value == classealterar.Situacao.Id.ToString());
                if (selectedItem != null)
                {
                    selectedItem.Selected = true;
                }

                if (usuario == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View("Formulario", classeinserir);
            }
            return View(id > 0 ? classealterar : classeinserir);
        }

        [HttpPost]
        public ActionResult Formulario(Usuario usuario)
        {
           
            if (ModelState.IsValid)
            {
                string Msg = "";
                try { if (string.IsNullOrWhiteSpace(usuario.Nome)) Msg += "Nome invalido"; } catch (Exception) { usuario.Nome = ""; }
                try { if (string.IsNullOrWhiteSpace(usuario.Senha)) Msg += "Senha invalida"; } catch (Exception) { usuario.Senha = ""; }
                try { if (string.IsNullOrWhiteSpace(usuario.SenhaConfirmar)) Msg += "Senha Confirmar invalida"; } catch (Exception) { usuario.SenhaConfirmar = ""; }

                if (Msg == "")
                {
                    string comparasenha = ComparaSenhas(usuario.Email, usuario.Senha, usuario.SenhaConfirmar);

                    if (!string.IsNullOrWhiteSpace(comparasenha) && (comparasenha != null) && (comparasenha != "")) { ViewBag.RetornoComparaSenha = comparasenha; } else { }

                    if (comparasenha != "") { return View(usuario); }

                    DateTime ultimaalteracao = DateTime.Now;
                    string sdtultimaalteracao = ultimaalteracao.ToString("yyyy-MM-dd H:mm:ss");
                    usuario.DataAlteracao = DateTime.Parse(sdtultimaalteracao);

                    contaAplicacao.Salvar(usuario);
                    return RedirectToAction("Busca", "Conta");
                }
                else
                {
                    return View(usuario);
                }
            }
            return RedirectToAction("Busca", "Conta", usuario);
        }

        public string Excluir(int id)
        {
            string Msg = "";

            if(id > 0)  
            {
                var usuario = contaAplicacao.ConsultaPorId(id);
                if (usuario == null) 
                { 
                    Msg = "Registro não encontrado"; 
                }

                contaAplicacao.Excluir(id);
                Msg = "Excluido com sucesso";
            }
            return Msg;
        }

        public List<SelectListItem> ConsultaSituacao(string parametro)
        {
            string Msg = "";
            List<SelectListItem> tempSituacao = new List<SelectListItem>();

            if (parametro != null)
            {
                var objsituacao = contaAplicacao.ConsultaSituacaoPorParametro(parametro);
                if (objsituacao == null) { Msg = "Situação não encontradao"; } else { Msg = ""; }

                foreach (var item in objsituacao)
                {
                    var items = objsituacao.Select(x => new SelectListItem 
                    {
                        Text = x.Texto,
                        Value = x.Valor.ToString(),
                    });

                    tempSituacao.AddRange(items);
                    break; 
                }
            }
            return tempSituacao;
        }
    }
}