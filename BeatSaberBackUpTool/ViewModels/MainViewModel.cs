using BeatSaberBackUpTool.Enums;
using BeatSaberBackUpTool.Interfaces;
using BeatSaberBackUpTool.Models;
using BeatSaberBackUpTool.Services;
using BeatSaberBackUpTool.Views;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Unity;

namespace BeatSaberBackUpTool.ViewModels
{
    public class MainViewModel : BindableBase, IInitializable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        [Dependency]
        public IDialogService dialogService_;
        [Dependency]
        public ILoadingService loadingService_;

        public Logger Logger => LogManager.GetCurrentClassLogger();

        /// <summary>バックアップ元 を取得、設定</summary>
        private string fromDirectory_;
        /// <summary>バックアップ元 を取得、設定</summary>
        public string FromDirectory
        {
            get { return this.fromDirectory_; }
            set { this.SetProperty(ref this.fromDirectory_, value); }
        }

        /// <summary>バックアップ先 を取得、設定</summary>
        private string toDirectory_;
        /// <summary>バックアップ先 を取得、設定</summary>
        public string ToDirectory
        {
            get { return this.toDirectory_ + this.FileName; }
            set { this.SetProperty(ref this.toDirectory_, value); }
        }

        /// <summary>結果メッセージ を取得、設定</summary>
        private string resultMessege_;
        /// <summary>結果メッセージ を取得、設定</summary>
        public string ResultMessege
        {
            get { return this.resultMessege_; }
            set { this.SetProperty(ref this.resultMessege_, value); }
        }

        /// <summary>作成中かどうか を取得、設定</summary>
        private bool isCreating_;
        /// <summary>作成中かどうか を取得、設定</summary>
        public bool IsCreating
        {
            get { return this.isCreating_; }
            set { this.SetProperty(ref this.isCreating_, value); }
        }
        private string FileName => "\\" + DateTime.Now.ToString("BSBackUp_yyyyMMddHHmmss") + ".zip";
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        /// <summary>ファイル保存ダイアログを開くコマンド を取得、設定</summary>
        private DelegateCommand<object> openDirectoryCommand_;
        /// <summary>ファイル保存ダイアログを開くコマンド を取得、設定</summary>
        public DelegateCommand<object> OpenDirectoryCommand { get { return this.openDirectoryCommand_ ?? (this.openDirectoryCommand_ = new DelegateCommand<object>(this.OpenDirectoryWindow)); } }

        /// <summary>バックアップコマンド を取得、設定</summary>
        private DelegateCommand backUpCommand_;
        /// <summary>バックアップコマンド を取得、設定</summary>
        public DelegateCommand BackUpCommand { get { return this.backUpCommand_ ?? (this.backUpCommand_ = new DelegateCommand(this.CreateZip)); } }

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        private void OpenDirectoryWindow(object param)
        {
            if (param is CommandType type) {
                var window = new CommonOpenFileDialog() { IsFolderPicker = true };
                if (window.ShowDialog() == CommonFileDialogResult.Ok) {
                    switch (type) {
                        case CommandType.From:
                            this.FromDirectory = window.FileName;
                            Properties.Settings.Default.BackUpFromDirectory = window.FileName;
                            this.Logger.Info("保存元を指定しました。");
                            break;
                        case CommandType.To:
                            this.ToDirectory = window.FileName;
                            Properties.Settings.Default.BackUpToDirectory = window.FileName;
                            this.Logger.Info("保存先を指定しました。");
                            break;
                        default:
                            break;
                    }
                    Properties.Settings.Default.Save();
                }
            }
        }
        private void CreateZip()
        {
            this.dialogService_.ShowDialog(nameof(DialogView), new DialogParameters() { { "Messege", "作成を始めました。" } }, _ => { });
            this.Logger.Info("作成を開始しました。");
            this.loadingService_.Create(this.domain_.Createzip);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // リクエスト
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(this.FromDirectory)) {
                this.domain_.FromPass = this.FromDirectory;
            }
            else if (args.PropertyName == nameof(this.ToDirectory)) {
                this.domain_.ToPass = this.ToDirectory;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        public void Initialize()
        {
            this.FromDirectory = Properties.Settings.Default.BackUpFromDirectory;
            this.ToDirectory = Properties.Settings.Default.BackUpToDirectory;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(
                this.loadingService_, nameof(INotifyPropertyChanged.PropertyChanged), this.OnLoadingServiceChanged);
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                this.loadingService_, nameof(INotifyPropertyChanged.PropertyChanged), this.OnLoadingServiceChanged);
        }
        private void OnLoadingServiceChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (sender is LoadingService service && e.PropertyName == nameof(service.IsCreating)) {
                this.IsCreating = service.IsCreating;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private readonly MainWindowDomain domain_;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        public MainViewModel()
        {
            this.domain_ = new MainWindowDomain();
            this.IsCreating = false;
            this.ResultMessege = "作成中です。しばらくお待ちください。";
        }
    }
}
