using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Titulo = "Perfil",
                TargetType = typeof(Perfil2)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Amigos",
                TargetType = typeof(Amigos)
            });

            this.Add(new MenuItem()
            {
                Titulo = "Configuraciones",
                TargetType = typeof(Configuraciones)
            });

        }
    }
}


