﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO.Ports;

namespace WPF_SerialPort
{
    //控件事件类
    public partial class MainWindow : Window
    {
        #region 按钮
        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            //刷新按钮对应事件
            ScanSerialPort(serialPort1, SerialPortComboBox);
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            //发送按钮对应事件
            byte[] Data = new byte[1];  //一次发送一个字节
            if (serialPort1.IsOpen || PostTextBox.Text == "")
            {
                if (PostValueRadioButton.IsChecked == false) //若发送模式是字符模式
                {
                    try
                    {
                        serialPort1.WriteLine(PostTextBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("串口数据写入错误", "错误");
                        serialPort1.Close();        //关闭串口
                        OpenButton.IsEnabled = true;  //打开端口按钮现已可按
                        CloseButton.IsEnabled = false;//关闭端口按钮不可再按
                    }
                }
                else                              //若发送模式是数值模式
                {
                    for (int i = 1; i < (PostTextBox.Text.Length - PostTextBox.Text.Length % 2) / 2; i++) //输入两个字母等于一个字节，故这里除以2
                    {
                        Data[0] = Convert.ToByte(PostTextBox.Text.Substring(i * 2, 2), 16);  //对输入框中的内容依次取2个，并转换为16进制byte型数值
                        serialPort1.Write(Data, 0, 1);    //对Data数组中的内容从偏移量为0的位置写入指定1个字节长度的内容
                    }
                    if (PostTextBox.Text.Length % 2 != 0) //若输入内容长度不为偶数
                    {
                        Data[0] = Convert.ToByte(PostTextBox.Text.Substring(PostTextBox.Text.Length - 1, 1), 16); //单独处理剩下的那个数值
                        serialPort1.Write(Data, 0, 1);
                    }
                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            //打开端口对应事件
            try
            {
                serialPort1.PortName = SerialPortComboBox.Text;                //串口名称
                serialPort1.BaudRate = Convert.ToInt32(BaudComboBox.Text, 10); //字符串型数值转换为十进制型赋给串口波特率
                serialPort1.Open();  //打开串口
                OpenButton.IsEnabled = false; //打开端口按钮不可再按
                CloseButton.IsEnabled = true; //关闭端口按钮现已可按
                PostButton.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("串口打开遇到错误", "错误");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //关闭端口对应事件
            try
            {
                serialPort1.Close();        //关闭串口
                OpenButton.IsEnabled = true;  //打开端口按钮现已可按
                CloseButton.IsEnabled = false;//关闭端口按钮不可再按
                PostButton.IsEnabled = false;
            }
            catch
            {
                //ignore
            }
        }
        #endregion

    }
}
