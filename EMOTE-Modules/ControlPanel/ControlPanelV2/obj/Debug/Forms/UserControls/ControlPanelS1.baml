   M S B A M L   `   `   `  ����G  CControlPanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ControlPanel.Forms.UserControlsControlPanel.Forms.UserControls  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      M IEmoteEvents, Version=1.0.5589.17802, Culture=neutral, PublicKeyToken=nullV:clr-namespace:EmoteEvents.ComplexData;assembly=EmoteEventsEmoteEvents.ComplexData 5      4    .ControlPanel.Forms.UserControls.ControlPanelS1   5      >UserControls-clr-namespace:ControlPanel.Forms.UserControls  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6sys&clr-namespace:System;assembly=mscorlib     5      Q
complxData:clr-namespace:EmoteEvents.ComplexData;assembly=EmoteEvents     5      $��371.125��5	   '     � 	Resources  5         StudentsDataTemplate �� Orientation �� 
TargetType �� Property �� Value S� 
MethodName S� 
ObjectType  System.Enum  ScenarioLanguages S� MethodParameters M� TypeName�� 5   
   %r  &	        &	 �    �� 5      .R�5      �� $ 
Horizontal=�6     5      ���� 5      '��       " ��6    .�5      ��  � ��$ 0,0,4,0q�67   6I   5      5      .+�5      �� ��6    �� *thalamusLearnerId+�� 5      �� 5      ��6    �� *	firstName+6=   �� 5      �� 5      ��6    �� *
middleName+6>   �� 5      �� 5      ��6    �� *
lastName+6<   �� 5      �� 5      ��6    �� *sex+67   5      5      S� 5       $ 	GetValues��6!   "  5!       5#      M� 5$      $" complxData:ScenarioLanguages��6   5&      5(      .��5+      ]� .+�5,   
   �� $	��Top=�6   $ Vertical=�6*   $��5q�6A     5-      ��%�   ']�       '�� L     �� 5.      " ]�6   .�5/      ��  � ��$ 5q�6/   6;   50      �� 51      " ��6   .�52      ��  � ��$ 70��6.   6;   53      54      .+�55      ]� .+�56      �� $
��Left=�6   $
��Wrap=�67   $��Session no :��6K   �� 57      -   5        	 ��Name$	 SessionNumberComboBox��57      59      ]� 5:      .+�5;      �� $
��Wrap=�6   $��	Student: ��60   �� 5<      -   5        $	 Student1ComboBox��5<      #��[  6V   
 �� SelectedIndex$
 0��6�   ��65   �� *
Learners+6�   5=      ]� 5>      $y�Hidden=�6   .+�5?      �� $��
Player 2: ��6   �� 5@      -   5        $	 Student2ComboBox��5@      #��[  6V   ��65   �� *
Learners+6V   5A      ]� 5B      .+�5C      �� $��
Scenario: ��6   �� 5D      -   5        $	 ScenarioComboBox��5D       �� 
IsEditable . $
 0��6G   5E      ]� 5F      .+�5G      �� $��Language��6   �� 5H      -   5        $	 LanguageComboBox��5H      $
 0��6w   ��65   ��  �� Source# [ 6w   5I      ]� 5J      .+�5K      �� $��
Empathic: ��6   �� 5L      -   5         ��Name$ IsEmpathicCheckBox��5L      5M      ]� 5N      .+�5O      �� $��	Auto end:��6   6/   �� 5P      -   5        $# IsEndingAutomaticallyCheckBox��5P       d� 	IsChecked$
 TrueY�6B   �� 5Q      $��10,0,0,0q�6   $ 
Horizontal=�6/   ��5R      �� 5S      " ��6    .�5T      ��  � y�$ Visible=�6;   6M    �� Triggers 5U      �� 5V   "    �� Binding 6.   ��  �� ElementName$# IsEndingAutomaticallyCheckBox�� �� Path$ 	IsChecked� �� Value	 False6|   .a�5W   &   ��  � y�$ Hidden=�6C   6T   5X   #   5Z      .+�5\      �� $��Duration (min):��6    $	��100��67   �� 5]      -   5         ��Name$ Duration��5]   4   $
��Left=�6   $��30��6F   :   4ControlPanel.Forms.UserControls.CountDownUserControl  5^      -	   5          Name$ CountDownUserControl��5^   J   $��20,0,0,0q�68   ��5_      �� 5`      "  6$   .�5a   "   ��  � y�$ Visible=�6?    5b   "   �� 5c   &    62   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >�$ Started� True6�   .a�5d   *   ��  � y�$ Visible=�6G   5e   '   5g      5i      5k      5l      5o      �� 5q   
   $��Bottom=�6   $ 
Horizontal=�6-   $��31��6F   $��Center=�6R   $��Bottom=�6o     5r      ��%Q   '��       �� 5s      " ��6   .�5t      ��  � ��$ 5q�6/   5u      5v      .+�5w      �� -
   5         ��Name$ ButtonStart��5w      $��Start��6*   $��Bottom=�6:   $��75��6U   $��5,0,5,6q�6|   ��5x      �� 5y      " ��6   .�5z      ��  � ��$ 5q�63   �� 5{       � {� .  6F    5|      �� 5}       6*   �� $ CountDownUserControl��$ Started�	 False6m   .a�5~   "   ��  � {� . 6>   5      5�      5�      �� 5�      -   5        $ 
ButtonStop��5�      $
��Stop��6)   $	��Top=�68   $��75��6P   6[   �� 5�      -   5        $��Reset��5�      $	��Top=�6%   $��75��6=   $y�Hidden=�6b   5�      5�      5�      