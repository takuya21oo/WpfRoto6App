using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfRoto6App
{
    public class Roto6ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Roto6InfoDto> _roto6InfoDto;

        public ObservableCollection<Roto6InfoDto> Roto6InfoDto
        {
            get => _roto6InfoDto;
            set
            {
                if (_roto6InfoDto != value)
                {
                    _roto6InfoDto = value;
                    OnPropertyChanged(nameof(Roto6InfoDto));
                }
            }
        }

        public ICommand SearchCommand { get; }
        
        public Roto6ViewModel()
        {
            Roto6InfoDto = new ObservableCollection<Roto6InfoDto>();
            SearchCommand = new RelayCommand(LoadRoto6);
        }


        //===============================================================================
        // 表示ボタン押下後に走る処理
        //===============================================================================
        private void LoadRoto6()
        {
            var dao = new Roto6Dao();
            var results = dao.DisplayGetRoto6Dao();
            Roto6InfoDto.Clear();
            foreach (var r in results)
            {
                Roto6InfoDto.Add(r);
            }            
        }


        //===============================================================================
        // 登録ボタン押下後の処理
        //===============================================================================
        public bool RegisterItemFromDatabase(Roto6InfoDto roto6InfoItem)
        {
            // フラグ
            bool vmResult = true;

            if (roto6InfoItem != null)
            {
                // DAOを呼び出して登録
                var dao = new Roto6Dao();
                // DBに登録
                vmResult = dao.InsertRoto6Dao(roto6InfoItem);

                if (vmResult == true)
                {
                    return vmResult;
                }
                else 
                {
                    return vmResult;
                }
            }
            else
            {
                return vmResult = false;
            }

        }


        //===============================================================================
        // 削除ボタン押下後の処理
        //===============================================================================
        public bool DeleteItemFromDatabase(Roto6InfoDto roto6InfoItem)
        {
            // フラグ
            bool vmResult = true;

            if (roto6InfoItem != null)
            {
                var dao = new Roto6Dao();
                // 🧨 DBから削除
                vmResult = dao.DeleteRoto6Dao(roto6InfoItem.Roto6No);
                
                if (vmResult == true)
                {
                    // 💡 UI反映のためにリストからも削除
                    Roto6InfoDto.Remove(roto6InfoItem);
                    return vmResult;
                }
                else
                {
                    return vmResult;
                }

            }
            else 
            {
                return vmResult = false;
            }

        }


        //===============================================================================
        // 
        //===============================================================================
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
