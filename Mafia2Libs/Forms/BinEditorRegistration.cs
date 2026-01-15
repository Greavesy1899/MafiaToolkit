using Core.IO;
using Mafia2Tool;
using System.IO;

namespace Mafia2Tool.Forms
{
    /// <summary>
    /// Registers all BIN file editors with the BinEditorFactory.
    /// This file lives in the Forms layer and registers editor factories,
    /// allowing Core.IO to create editors without directly depending on Forms.
    /// </summary>
    public static class BinEditorRegistration
    {
        /// <summary>
        /// Initialize all BIN editor registrations. Call this once during application startup.
        /// </summary>
        public static void Initialize()
        {
            // Register magic-based editors
            BinEditorFactory.RegisterMagicEditor(FileBin.CityAreasMagic, file =>
            {
                new CityAreaEditor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.CityShopsMagic, file =>
            {
                new CityShopEditor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.ShopMenu2Magic, file =>
            {
                new ShopMenu2Editor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.StreamMapMagic, file =>
            {
                new StreamEditor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.CGameMagic, file =>
            {
                new CGameEditor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.SDSConfigMagic, file =>
            {
                new SdsConfigEditor(file);
                return true;
            });

            BinEditorFactory.RegisterMagicEditor(FileBin.FramePropsMagic, file =>
            {
                new FramePropsEditor(file);
                return true;
            });

            // Register filename-based editors (no magic number)
            BinEditorFactory.RegisterMatcherEditor(
                FileBin.IsGameParamsFile,
                file =>
                {
                    new GameParamsEditor(file);
                    return true;
                });
        }
    }
}
