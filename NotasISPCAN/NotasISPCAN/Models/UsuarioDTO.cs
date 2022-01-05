using System;
using System.Collections.Generic;
using System.Text;

namespace NotasISPCAN.Models
{
    public class UsuarioDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public string Telefone { get; set; }
        public string Categoria { get; set; }
    }
}
