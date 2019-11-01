using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class TeachersListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private readonly IIPSASDbContext dbContext;

        private IList<TeacherViewModel> _teachers;

        private ICommand _clickCommand;
        public ICommand DeselectTeacherCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => SelectedTeacher = null, () => true));
            }
        }

        public IList<TeacherViewModel> Teachers
        {
            get
            {
                return _teachers;
            }

            set
            {
                _teachers = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Teachers)));
            }
        }

        private Teacher _teacher;

        public Teacher SelectedTeacher 
        {
            get 
            {
                return _teacher;
            }
            set
            {
                _teacher = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTeacher)));
            }
        }


        public TeacherViewModel SelectedTeacherVM
        {
            get
            {
                if (SelectedTeacher != null)
                {
                    return TeacherViewModel.FromTeacher(SelectedTeacher);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    SelectedTeacher = GetTeacherById(value.Id);
                }
            }
        }

        public Teacher GetTeacherById(int id)
        {
            return dbContext.Teachers.FirstOrDefault(t => t.Id == id);
        }

        public TeachersListViewModel(IPSASDbContext dbContext)
        {
            this.dbContext = dbContext;
            LoadTeachers();
        }

        public void LoadTeachers()
        {
            _teachers = dbContext.Teachers
                .Select(t => TeacherViewModel.FromTeacher(t)).ToList();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Teachers)));
        }
    }
}
