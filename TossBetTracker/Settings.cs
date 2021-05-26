using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;

namespace TossBetTracker
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters.
    /// </summary>
    public static class Settings
    {


        private static ISettings AppSettings =>
        CrossSettings.Current;

        public static string Idusuario
        {
            get => AppSettings.GetValueOrDefault(nameof(Idusuario), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Idusuario), value);
        }

        public static string Nombre
        {
            get => AppSettings.GetValueOrDefault(nameof(Nombre), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Nombre), value);
        }

        public static string Foto
        {
            get => AppSettings.GetValueOrDefault(nameof(Foto), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Foto), value);
        }

        public static string Telefono
        {
            get => AppSettings.GetValueOrDefault(nameof(Telefono), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Telefono), value);
        }

        public static string Correo
        {
            get => AppSettings.GetValueOrDefault(nameof(Correo), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Correo), value);
        }

        public static string OneSignal
        {
            get => AppSettings.GetValueOrDefault(nameof(OneSignal), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(OneSignal), value);
        }

        public static string Ancho
        {
            get => AppSettings.GetValueOrDefault(nameof(Ancho), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Ancho), value);
        }

        public static string Deporte
        {
            get => AppSettings.GetValueOrDefault(nameof(Deporte), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Deporte), value);
        }

        public static string Partido
        {
            get => AppSettings.GetValueOrDefault(nameof(Partido), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Partido), value);
        }

        public static string Remover
        {
            get => AppSettings.GetValueOrDefault(nameof(Remover), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Remover), value);
        }

        public static string tickets
        {
            get => AppSettings.GetValueOrDefault(nameof(tickets), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(tickets), value);
        }

        public static string Momio
        {
            get => AppSettings.GetValueOrDefault(nameof(Momio), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Momio), value);
        }

        public static string Notif
        {
            get => AppSettings.GetValueOrDefault(nameof(Notif), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Notif), value);
        }

        public static string Primera
        {
            get => AppSettings.GetValueOrDefault(nameof(Primera), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Primera), value);
        }

    }
}
