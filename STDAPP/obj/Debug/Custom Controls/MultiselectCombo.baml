   M S B A M L   `   `   `  ����
  STDAPP    STDAPP.MultiselectCombo   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      $��30��5      $	��150��6     � 	Resources  5         boolToVisibility��%   &	        �� 5   
   6'   5      .��5	      ��  �� CornerRadius$ 5��6   $��2q�6   
�������$��	0,0,-31,0q�6E   ._�5
   
   � ��5      �� 5       � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��32 �6#   .+�5      �� $��0��6   $��0��6"   
���   �$ 5��6E   $��2q�6V   ._�5      �� -   5         ��Name$ 
txtContent��5       �� 
IsReadOnly . 
���   �
�������$��0q�6f   $��Segoe UI Black)�6z   $��15(�6�   6�   5      d� 5      -   5         d�Name$ btnDropDown��5      $��0��60   $��1��6=   
�������$��0q�6a   $��0,2,2,2q�6u   |�.    CustomTogel#��[ 6�   5      � 5      $��0��6   $��1��6    $��Stretch=�60   $��Stretch=�6N   y�5      ��  �� Path$ IsOpen� �� 	Converter# [   �� ElementName$ pupData��5      � 5      -   5        	 �Name$	 pupData��5      $	��150��6%   $	��300��61   
 � 	StaysOpen
 .  $��Slide=�6�    � AllowsTransparency . $	��250��6�   ��6P   �� $ 	IsChecked�$ btnDropDown��.��5      �� $��1q�6   
�������
���   �$ 5��6U   ._�5       �� -   5         ��Name$ lstComboItems��5        �� SelectionMode$ Multiple=�65   $��Segoe UI Black)�6�   $��16(�6�   
���   �
���������5!      �� 5"       �� 
TargetType" ��6$   .�5#   "   ��  �� Property � �� �� Value 5$   &   �� 5%   *    �� 
TargetType" ��6:   .R�5&   .   �� #��z��65   $��0q�6_   $��2q�6s   ._�5'   2   �� #��z��6<   $��2q�6]   5(   /   5)   +   5+   #    �� Triggers 5-   "   P� 5.   &    P� Property �@��
IsSelected P� Value . . �5/   *   ��  � �� �#3637396G   50   '   P� 51   &    �@J�IsMouseOver . . �52   *   ��  � �� �#8657726G   53   '   57        59      ��%^   '��       �� 5:      " ��6$   .�5;   "   ��  �@��CornerRadius$ 5��6A   5<      5=      .@�5>      �� $��Random(Lamoda)��6&   $��14(�6?   �� 5?      $	��3.5��6&   �� 5@      $��4��6&   �� 5A      $	��4.5��6&   �� 5B      $��5��6&   �� 5C      $	��5.5��6&   �� 5D      $��6��6&   �� 5E      $	��6.5��6&   �� 5F      $��7��6&   �� 5G      $	��7.5��6&   �� 5H      $��8��6&   �� 5I      $	��8.5��6&   �� 5J      $��9��6&   �� 5K      $	��9.5��6&   �� 5L      $��10��6&   �� 5M      $
��10.5��6&   �� 5N      $��11��6&   �� 5O      $
��11.5��6&   �� 5P      $��12��6&   �� 5Q      $
��12.5��6&   �� 5R      $��13��6&   �� 5S      $
��13.5��6&   �� 5T      $��14��6&   �� 5U      $
��14.5��6&   �� 5V      $��15��6&   �� 5W      $
��3.5Y��6&   �� 5X      $��4Y��6&   �� 5Y      $
��4.5Y��6&   �� 5Z      $��5Y��6&   �� 5[      $
��5.5Y��6&   �� 5\      $��6Y��6&   �� 5]      $
��6.5Y��6&   �� 5^      $��7Y��6&   �� 5_      $��XS��6&   �� 5`      $��S��6&   �� 5a      $��M��6&   �� 5b      $��L��6&   �� 5c      $��XL��6&   5d      5e      5f      5g      5h      5i      