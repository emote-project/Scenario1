   M S B A M L   `   `   `  ���� I  ENAOThalamusGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NAOThalamusGUINAOThalamusGUI  J FSshShellControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SshShellControl;assembly=SshShellControlSshShellControl &     NAOThalamusGUI.NAOThalamusStatus   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     .localclr-namespace:NAOThalamusGUI     RSshShellControl6clr-namespace:SshShellControl;assembly=SshShellControl     -     � Title$  NAOThalamusStatus��$	��351��$	��622�� � DataContext    NAOThalamusGUI.NAOStatus   � 	Resources    
OnOffField �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%   &	        �� " �� P�  � ��$
 True��. ���  � ��
 � � �P�  � ��$ False��. ���  � ��
 �  ��.��� .+�%  SshShellControl.SshShellControl  -   	  Name$	 shellControl��$��9,144,10,10q�$
��Auto��$
��Auto���� $��Status��$��	9,7,163,0q�$	��Top=�$	��132��.���� 
 �� Orientation$
 Vertical=�$��4q�.+��� $
 
Horizontal=�$
��Auto��.+��� $
��Wrap=�$��80��$	��NAO���� $
��Wrap=����� .���  � ��$$ Searching for connected NAOs..�� ��  �� Binding �� *IsSearchingForNAO+ �� Value	 False.a���  � �� �� *SelectedNAO.Name+��  � ��
 � � ���  �� *IsSearchingForNAO+ True.a���  � ��$$ Searching for connected NAOs..���� $
 
Horizontal=�$
��Auto��.+��� $
��Wrap=�$��80��$��Python���� $
��Wrap=�$
��Auto������ " �� �� BasedOn# [   ��  �� *IsPythonInstalled+	 False.a���  � ��$' !Python not installed on the robot����  � ��
 �  ����  �� *IsPythonRunning+ True.a���  � ��$ Running����  � ��
 � � ���  �� *IsPythonRunning+	 False.a���  � ��$ Not running����  � ��
 �  ���� -    ��Name$ btnInstallPython��$��Install��$��10,0,0,0q����� " �� ��  �� *IsPythonInstalled+	 False.a���  � y�$ Visible=���  �� *IsPythonInstalled+ True.a���  � y�$ Hidden=���  �� *IsPythonInstalling+ True.a���  � {� .  ��  �� *IsPythonInstalling+	 False.a���  � {� . ��  �� *IsSearchingForNAO+ True.a���  � y�$ Hidden=��� $��10,0,0,0q����� *PythonUploadStatusDescription+�� $
 
Horizontal=�$
��Auto��.+��� $
��Wrap=�$��80��$��
Behaviours���� $
��Wrap=�$
��Auto������ " �� ��  �� *IsBehaviourInstalled+ True.a���  � ��$ 	Installed����  � ��
 � � ���  �� *IsBehaviourInstalled+	 False.a���  � ��$ Not installed����  � ��
 �  ���� -   $ btnInstallBehaviours��$��Install��$��10,0,0,0q����� " �� ��  �� *IsBehaviourInstalled+	 False.a���  � y�$ Visible=���  �� *IsBehaviourInstalled+ True.a���  � y�$ Hidden=���  �� *IsBehaviourInstalling+ True.a���  � {� .  ��  �� *IsBehaviourInstalling+	 False.a���  � {� . ��  �� *IsBehaviourChecking+ True.a���  � y�$ Hidden=���  �� *IsSearchingForNAO+ True.a���  � y�$ Hidden=��� $��10,0,0,0q����� *#!BehavioursUploadStatusDescription+�� $
 
Horizontal=�$
��Auto��.+��� $
��Wrap=�$��80��$��NaoQi���� $
��Wrap=�����  ��  �� *IsNAOQiRunning+	 False.a���  � ��$ Not Running����  � ��
 �  ����  �� *IsNAOQiRunning+ True.a���  � ��$ Running����  � ��
 � � ��� $
 
Horizontal=�$
��Auto��.+��� $
��Wrap=�$��80��$��	Thalamus:���� $
��Wrap=�����  ��  �� *IsThalamusConnected+	 False.a���  � ��$ Connecting..����  �� *IsThalamusConnected+ True.a���  � ��$ 	Connected����  � ��
 � � ��� $��Controls��$��Right=�$��	0,10,10,0q�$	��148��$	��100��$	��Top=�.���� $
 Vertical=�$��3q� ��%�   '��       �� " ��.���  � ��$ 1q���  � ��$ 9(� ��  �� *SelectedNAO+ Z� .a���  � {� .  .+��� -   $ cmdStartPython��$��Python: Start���� -   $ cmdStopPython��$��Python: Stop���� -   $ cmdKillPython��$��Python: Kill���� -   $ cmdRestartNaoQi��$��NaoQi: Restart��