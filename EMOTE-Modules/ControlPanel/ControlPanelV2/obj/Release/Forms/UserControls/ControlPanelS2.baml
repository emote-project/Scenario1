   M S B A M L   `   `   `  ���� G  CControlPanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ControlPanel.Forms.UserControlsControlPanel.Forms.UserControls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem M IEmoteEvents, Version=1.0.5714.22310, Culture=neutral, PublicKeyToken=nullV:clr-namespace:EmoteEvents.ComplexData;assembly=EmoteEventsEmoteEvents.ComplexData 4    .ControlPanel.Forms.UserControls.ControlPanelS2   >UserControls-clr-namespace:ControlPanel.Forms.UserControls  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     Q
complxData:clr-namespace:EmoteEvents.ComplexData;assembly=EmoteEvents       � 	Resources     StudentsDataTemplate �� Orientation �� 
TargetType �� Property �� Value S� 
MethodName S� 
ObjectType  System.Enum  ScenarioLanguages S� MethodParameters M� TypeName�� %X  &	        &	 �     �� .R��� $ 
Horizontal=�  ���� '��       " ��.���  � ��$ 0,0,4,0q�.+��� ���� *thalamusLearnerId+�� �� ���� *	firstName+�� �� ���� *
middleName+�� �� ���� *
lastName+�� �� ���� *sex+S� $ 	GetValues��"   M� $" complxData:ScenarioLanguages��.��]� .+��� $	��Top=�$ Vertical=�$��5q�  ��%Y   ']�       '�� "     �� " ]�.���  � ��$ 5q��� " ��.���  � ��$ 70��.+�]� .+��� $
��Left=�$
��Wrap=�$��Session n. :���� -   	 ��Name$	 SessionNumber��.@��� .��1�� .��2�� .��3�� .��4]� .+��� $
��Wrap=�$��
Player 1: ���� -   $	 Student1ComboBox��#��[  ���� *
Learners+]� .+��� $��
Player 2: ���� -   $	 Student2ComboBox��#��[  ���� *
Learners+]� .+��� $��
Empathic: ���� -   
 ��Name$
 IsEmpathicCheckBox��]� .+��� $��	Auto end:���� -   $#
 IsEndingAutomaticallyCheckBox�� d� 	IsChecked$
 TrueY��� $��10,0,0,0q�$ 
Horizontal=����� " ��.���  � y�$ Visible=� �� Triggers ��  �� Binding ��  �� ElementName$# IsEndingAutomaticallyCheckBox�� �� Path$ 	IsChecked� �� Value	 False.a���  � y�$ Hidden=�.+��� $��Duration (min):��$	��100���� -    ��Name$ Duration��$
��Left=�$��30��$��20��:   4ControlPanel.Forms.UserControls.CountDownUserControl  -     Name$ CountDownUserControl��$��20,0,0,0q����� "  .���  � y�$ Visible=� ��  ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" >�$ Started� True.a���  � y�$ Visible=��� $��Bottom=�$��Right=��� $��Bottom=�$ 
Horizontal=�$��31��$��Center=�$��Bottom=�  ��%,   '��       �� " ��.���  � ��$ 5q�.+��� -   $��Start��$	��Top=�$��75������ " ��.���  � ��$ 5q���  � {� .   ��  �� $ CountDownUserControl��$ Started�	 False.a���  � {� . �� -	   $��End Game��$	��Top=�$��75��{��� $ CountDownUserControl��$ Started�