   M S B A M L   `   `   `  ���� G  CControlPanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ControlPanel.viewModelsControlPanel.viewModels  U/clr-namespace:ControlPanel.Thalamus.UserControl!ControlPanel.Thalamus.UserControl  '    !ControlPanel.Forms.DatabaseWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     <
viewModels%clr-namespace:ControlPanel.viewModels     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     GUserControl/clr-namespace:ControlPanel.Thalamus.UserControl       � Title$  Student Database��$	��349��$	��728�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterOwner=� � Icon$  ../Assets/ControlPanel.ico�� � DataContext 1   +ControlPanel.viewModels.StudentsDBViewModel  .��� .+�&   System.Windows.Controls.DataGrid  -     Name$ MainGrid��$��	0,36,0,81q�   AutoGenerateColumns .  ���� *
Learners+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 SizeToCells 
   
IsReadOnly
 .    MinWidth$ 50Q�   CellTemplate �� .R��� ���� *	firstName+  $ Middle Name��$	 SizeToCells 
 . $ 50Q� �� .R��� ���� *
middleName+  $ 	Last Name��$	 SizeToCells 
 . $ 50Q� �� .R��� ���� *
lastName+  $ Birth��$	 SizeToCells 
 . $ 50Q� �� .R��� ���� *birth+  $	 Sex��$	 SizeToCells 
 . $ 50Q� �� .R��� ���� *sex+� $��46��$��Bottom=�$��0,0,0,30q�.+��� -    ��Name$ txtFirstName��$��
10,16,0,10q�$
��Left=�$	��112���� $
��Left=�$��10,0,0,0q�$
��Wrap=�$��
First Name��$	��Top=�$	��112���� $
��Left=�$��	127,0,0,0q�$
��Wrap=�$��Middle Name��$	��Top=�$	��112���� -   $ txtMiddleName��$��127,16,0,10q�$
��Left=�$	��112���� $
��Left=�$��	244,0,0,0q�$
��Wrap=�$��	Last Name��$	��Top=�$	��112���� -   $ txtLastName��$��244,16,0,10q�$
��Left=�$	��112���� $
��Left=�$��	361,0,0,0q�$
��Wrap=�$��Birth��$	��Top=�$	��112��(  "System.Windows.Controls.DatePicker  -     Name$ DatePickerBirth��$
��Left=�$��
361,16,0,0q�$	��Top=�$	��112���� -    ��Name$ cmbSex��$
��Left=�$��
478,16,0,0q�$	��Top=�$��34��$��20��.@��� $��M���� $��F���� $
��Left=�$��	478,0,0,0q�$
��Wrap=�$	��Sex��$	��Top=�$��34���� -    ��Name$ 	AddButton��$	��Add��$��50��$��	0,16,65,0q�$��Right=�$��20��$	��Top=��� -   $ RemoveButton��$��Remove��$��50��$��	0,16,10,0q�$��Right=�$��20��$	��Top=�����  �� 
TargetType" �� �� Triggers ��  �� Binding �� *SelectedIndex+ �� ElementName$ MainGrid�� �� Value -1.a���  �� Property � {� �� Value .  �� $��25��$��Bottom=�.@��� .���� -	    ��Name$ DatabaseStatus��$"Waiting for Thalamus to connect...�� -
   $ ImportButton��$��Import��$��50��$��	0,10,10,0q�$��Right=�$��20��$	��Top=��� -   $ RefreshButton��$��Refresh�� � ToolTip$. (Refresh list with data from the database��$��50��$��	0,10,65,0q�$��Right=�$��20��$	��Top=�