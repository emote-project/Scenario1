   M S B A M L   `   `   `  ����   WOZInterface    WOZInterface.GazeControl   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources     	GlanceAOn  
GlanceAOff  	GlanceBOn  
GlanceBOff  GazeAOn  GazeAOff  GazeBOn  GazeBOff  GazeCOn 	 GazeCOff 
 GazeDOn  GazeDOff  PointAOn  	PointAOff  PointBOn  	PointBOff  GlanceRadioButton �� 
TargetType �� BitmapScalingMode �� Triggers ^� 
Conditions �� Binding �� RelativeSource �� Path �� Value	 �� Value
 �� Property  GazeRadioButton  PointRadioButton��%+  &	        &	 H     &	 �     &	 �     &	 "    &	 h    &	 �    &	 �    &	 <    &		 �    &	
 �    &	     &	 V    &	 �    &	 �    &	 ,    &	 t    &	     &	 �    �� $B��<pack://application:,,,/Images/GazePointControl/GlanceAOn.png���� $C��=pack://application:,,,/Images/GazePointControl/GlanceAOff.png���� $B��<pack://application:,,,/Images/GazePointControl/GlanceBOn.png���� $C��=pack://application:,,,/Images/GazePointControl/GlanceBOff.png���� $@��:pack://application:,,,/Images/GazePointControl/GazeAOn.png���� $A��;pack://application:,,,/Images/GazePointControl/GazeAOff.png���� $@��:pack://application:,,,/Images/GazePointControl/GazeBOn.png���� $A��;pack://application:,,,/Images/GazePointControl/GazeBOff.png���� $@��:pack://application:,,,/Images/GazePointControl/GazeCOn.png���� $A��;pack://application:,,,/Images/GazePointControl/GazeCOff.png���� $@��:pack://application:,,,/Images/GazePointControl/GazeDOn.png���� $A��;pack://application:,,,/Images/GazePointControl/GazeDOff.png���� $A��;pack://application:,,,/Images/GazePointControl/PointAOn.png���� $B��<pack://application:,,,/Images/GazePointControl/PointAOff.png���� $A��;pack://application:,,,/Images/GazePointControl/PointBOn.png���� $B��<pack://application:,,,/Images/GazePointControl/PointBOff.png���� " 	�.R�� .+��� $	��160��$	��150��$ NearestNeighbor=���  ^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� 	GlanceARB.0��� #	 �   
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� 	GlanceARB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� 	GlanceBRB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� 	GlanceBRB.0��� #	 �  
 � ���� " 	�.R�� .+��� $	��160��$	��150��$ NearestNeighbor=���  ^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� GazeARB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� GazeARB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� GazeBRB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� GazeBRB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� GazeCRB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� GazeCRB.0��� #	 � 	 
 � ��^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� GazeDRB.0��� #	 � 
 
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� GazeDRB.0��� #	 �  
 � ���� " 	�.R�� .+��� $��85��$��85��$ NearestNeighbor=���  ^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� PointARB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� PointARB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked� True��  ��  �� *self+$
 Name� PointBRB.0��� #	 �  
 � ��^�  ��  ��  �� *self+$ 	IsChecked�	 False��  ��  �� *self+$
 Name� PointBRB.0��� #	 �  
 � ��.��� $��0,0,0,10q�.+�� -    �Name$ Glance��$��0,0,0,21q�.+�	� -   $��	GlanceARB�� d� 	IsChecked$
 TrueY�$��Center=�#��[ $	��100��$��0,0,200,245q�	� -   $��	GlanceBRB��$ FalseY�$��Center=�#��[ $	��100��$��200,0,0,245q�� -   $
 Gaze��$��0,0,0,53q�.+�	� -   $��GazeARB��$
 TrueY�$��Center=�#��[ $	��100��$��49,48,151,153q�	� -   $��GazeBRB��$ FalseY�$��Center=�#��[ $	��100��$��149,48,51,153q�	� -   $��GazeCRB��$ FalseY�$��Center=�#��[ $	��100��$��49,144,151,57q�	� -   $��GazeDRB��$ FalseY�$��Center=�#��[ $	��100��$��149,144,51,57q�� -	   $ Point��$��0,0,0,53q�.+�	� -
   $��PointARB��$
 TrueY�$��Center=�#��[ $��85��$��30��$��106,102,164,110q�	� -   $��PointBRB��$ FalseY�$��Center=�#��[ $��85��$��30��$��159,102,111,110q�� $��
0,233,0,21q�.+��� -   $��OverideGlanceCK��$ FalseY�$��Center=�  GlanceCheck#��[ $��76��$��	0,0,224,0q��� -   $��OverideGazeCK��$ FalseY�$��Center=�  	GazeCheck#��[ $��76��$��116,0,108,0q��� -   $��OveridePointCK��$ FalseY�$��Center=�  
PointCheck#��[ $��76��$��	224,0,0,0q�