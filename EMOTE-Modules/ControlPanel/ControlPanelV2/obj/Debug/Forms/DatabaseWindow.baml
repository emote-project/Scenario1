   M S B A M L   `   `   `  ����G  CControlPanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ControlPanel.viewModelsControlPanel.viewModels  5   	   U/clr-namespace:ControlPanel.Thalamus.UserControl!ControlPanel.Thalamus.UserControl  5   	   '    !ControlPanel.Forms.DatabaseWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   <
viewModels%clr-namespace:ControlPanel.viewModels     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   GUserControl/clr-namespace:ControlPanel.Thalamus.UserControl     5   	     � Title$  Student Database��5	   	   $	��349��6"   $	��728��6/    � 
ResizeMode$ NoResize=�6;    � WindowStartupLocation$ CenterOwner=�6Q    � Icon$  ../Assets/ControlPanel.ico��6u    � DataContext 5      1   +ControlPanel.viewModels.StudentsDBViewModel  5   
   .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ MainGrid��5      $��	0,36,0,81q�6F      AutoGenerateColumns .  ��6%   �� *
Learners+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6)   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 SizeToCells 67   
   
IsReadOnly
 .    MinWidth$ 50Q�6]      CellTemplate 5      �� 5      .R�5      �� ��6(   �� *	firstName+5      5        5      $ Middle Name��6)   $	 SizeToCells 6>   
 . $ 50Q�6d    5      �� 5      .R�5      �� ��6(   �� *
middleName+5       5"        5#      $ 	Last Name��6)   $	 SizeToCells 6<   
 . $ 50Q�6b    5$      �� 5%      .R�5&      �� ��6(   �� *
lastName+5'      5)        5*      $ Birth��6)   $	 SizeToCells 68   
 . $ 50Q�6^    5+      �� 5,      .R�5-      �� ��6(   �� *birth+5.      50        51      $	 Sex��6)   $	 SizeToCells 66   
 . $ 50Q�6\    52      �� 53      .R�54      �� ��6(   �� *sex+55      57      59      � 5;   
   $��46��6   $��Bottom=�6   $��0,0,0,30q�66   .+�5<      �� -   5         ��Name$ txtFirstName��5<      $��
10,16,0,10q�6,   $
��Left=�6@   $	��112��6[   �� 5=      $
��Left=�6   $��10,0,0,0q�63   $
��Wrap=�6E   $��
First Name��6Y   $	��Top=�6k   $	��112��6�   �� 5>      $
��Left=�6   $��	127,0,0,0q�63   $
��Wrap=�6F   $��Middle Name��6Z   $	��Top=�6m   $	��112��6�   �� 5?      -   5        $ txtMiddleName��5?      $��127,16,0,10q�6-   $
��Left=�6B   $	��112��6]   �� 5@      $
��Left=�6   $��	244,0,0,0q�63   $
��Wrap=�6F   $��	Last Name��6Z   $	��Top=�6k   $	��112��6�   �� 5A      -   5        $ txtLastName��5A      $��244,16,0,10q�6+   $
��Left=�6@   $	��112��6[   �� 5B      $
��Left=�6   $��	361,0,0,0q�63   $
��Wrap=�6F   $��Birth��6Z   $	��Top=�6g   $	��112��6   (  "System.Windows.Controls.DatePicker  5C      -   5          Name$ DatePickerBirth��5C      $
��Left=�62   $��
361,16,0,0q�6M   $	��Top=�6a   $	��112��6y   �� 5D      -   5         ��Name$ cmbSex��5D      $
��Left=�6'   $��
478,16,0,0q�6B   $	��Top=�6V   $��34��6n   $��20��6y   .@�5E      �� $��M��6   �� 5F      $��F��6   5G      �� 5H      $
��Left=�6   $��	478,0,0,0q�63   $
��Wrap=�6F   $	��Sex��6Z   $	��Top=�6e   $��34��6}   �� 5I      -   5         ��Name$ 	AddButton��5I      $	��Add��6(   $��50��66   $��	0,16,65,0q�6A   $��Right=�6T   $��20��6p   $	��Top=�6|   6�   �� 5J      -   5        $ RemoveButton��5J      $��Remove��6+   $��50��6<   $��	0,16,10,0q�6G   $��Right=�6Z   $��20��6v   $	��Top=�6�   ��5K      �� 5L       �� 
TargetType" ��6    �� Triggers 5M      �� 5N       �� Binding 6*   �� *SelectedIndex+ �� ElementName$ MainGrid�� �� Value -16c   .a�5O   "   ��  �� Property � {� �� Value .  6>   5P      5R      5T      5W      �� 5\   
   $��25��6   $��Bottom=�6    .@�5]      �� .��5^      �� -	   5         ��Name$ DatabaseStatus��5^      $"Waiting for Thalamus to connect...6X   5_      5`      �� 5a   
   -
   5        $ ImportButton��5a      $��Import��6'   $��50��68   $��	0,10,10,0q�6C   $��Right=�6V   $��20��6r   $	��Top=�6~   6�   �� 5b   
   -   5        $ RefreshButton��5b      $��Refresh��6(    � ToolTip$. (Refresh list with data from the database��6:   $��50��6m   $��	0,10,65,0q�6x   $��Right=�6�   $��20��6�   $	��Top=�6�   6�   5f      5g      