using FASTsim.Library.SECS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static FASTsim.Library.SECS.CommonData;

namespace FASTsim.GUI
{
    public partial class OtherLevel : CommonControl
    {
        public event EventHandler SendMsgEvent;
        public OtherLevel()
        {
            InitializeComponent();
            SendMsgEvent += FASTsim.SaveChildMessage;
        }

        internal override void MainFormNodeChaned(object sender, EventArgs e)
        {
            MyEventArg arg = e as MyEventArg;
            SECSItem secsItem = (SECSItem)arg.Value;
            level9Name.Text = secsItem.Name;
            level9Descr.Text = secsItem.Description;
            for(int i=0; i< level9Format.Items.Count; i++)
            {
                if(level9Format.Items[i].ToString() == secsItem.Format.ToString())
                {
                    level9Format.SelectedIndex = i;
                }
            }
            if(secsItem.NLB == 0)
            {
                level9NLB.SelectedIndex = 0;
            }
            else
            {
                for (int i = 1; i < level9NLB.Items.Count; i++)
                {
                    if (level9NLB.Items[i].ToString() == secsItem.NLB.ToString())
                    {
                        level9NLB.SelectedIndex = i;
                    }
                }
            }

            if (secsItem.Format == SECS_FORMAT.LIST)
            {
                level9Value.Visible = false;
                level9CheckBox.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                level9Format.Visible = false;
                label5.Visible = false;
                level9NLB.Visible = false;
            }
            else
            {
                if(secsItem.Value == null)
                {
                    level9Value.Text = "";
                }
                else
                {
                    if(secsItem.Value.GetType().BaseType == typeof(Array))
                    {
                        string str;
                        switch (secsItem.Format)
                        {
                            case SECS_FORMAT.I1:
                                str = string.Join(" ", (sbyte[])secsItem.Value);
                                break;
                            case SECS_FORMAT.I2:
                                str = string.Join(" ", (short[])secsItem.Value);
                                break;
                            case SECS_FORMAT.I4:
                                str = string.Join(" ", (int[])secsItem.Value);
                                break;
                            case SECS_FORMAT.I8:
                                str = string.Join(" ", (long[])secsItem.Value);
                                break;
                            case SECS_FORMAT.F8:
                                str = string.Join(" ", (double[])secsItem.Value);
                                break;
                            case SECS_FORMAT.F4:
                                str = string.Join(" ", (float[])secsItem.Value);
                                break;
                            case SECS_FORMAT.U1:
                                str = string.Join(" ", (byte[])secsItem.Value);
                                break;
                            case SECS_FORMAT.U2:
                                str = string.Join(" ", (ushort[])secsItem.Value);
                                break;
                            case SECS_FORMAT.U4:
                                str = string.Join(" ", (uint[])secsItem.Value);
                                break;
                            case SECS_FORMAT.U8:
                                str = string.Join(" ", (ulong[])secsItem.Value);
                                break;
                            default:
                                str = string.Join(" ", (byte[])secsItem.Value);
                                break;
                        }
                        level9Value.Text = str;
                    }
                    else
                    {
                        level9Value.Text = secsItem.Value.ToString();
                    }
                }
            }
        }

        private void SendMessage()
        {
            try
            {
                if (SendMsgEvent != null)
                {
                    Dictionary<string, object> valueDic = new Dictionary<string, object>();
                    valueDic["name"] = level9Name.Text;
                    valueDic["desc"] = level9Descr.Text;
                    if (!level9Format.Visible)
                    {
                        SendMsgEvent(this, new MyEventArg() { Value = valueDic });
                        return;
                    }
                    else if(level9Value.Text == "")
                    {

                        valueDic["value"] = null;
                        valueDic["format"] = level9Format.SelectedItem.ToString();
                        valueDic["nbl"] = level9NLB.SelectedItem.ToString();
                    }
                    else
                    {
                        if (level9Format.Visible || level9NLB.Visible || level9Value.Visible)
                        {
                            valueDic["format"] = level9Format.SelectedItem.ToString();
                            valueDic["nbl"] = level9NLB.SelectedItem.ToString();

                            string format = level9Format.Text;
                            if (format == "ASCII" || format == "JIS8")
                            {
                                if (level9CheckBox.Checked)
                                {
                                    int value = int.Parse(level9Value.Text);
                                    if (0 <= value && value < 256)
                                    {
                                        valueDic["value"] = value;
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                else if (format == "BINARY")
                                {
                                    int value = int.Parse(level9Value.Text);
                                    if (0 <= value && value < 256)
                                    {
                                        //object v = value.ToString("X");
                                        valueDic["value"] = value;
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                else
                                {
                                    valueDic["value"] = level9Value.Text;
                                }
                            }
                            else
                            {
                                if (format != "BOOLEAN")
                                {
                                    if (level9CheckBox.Checked)
                                    {
                                        string[] numList = level9Value.Text.Trim().Split(new char[] { ' ', ',' });
                                        int[] valueArray = new int[numList.Length];
                                        for (int i = 0; i < numList.Length; i++)
                                        {
                                            valueArray[i] = int.Parse(numList[i]);
                                        }
                                        valueDic["value"] = valueArray;
                                    }
                                    else
                                    {
                                        int value = int.Parse(level9Value.Text);
                                        valueDic["value"] = value;
                                    }
                                }
                                else
                                {
                                    valueDic["value"] = level9Value.Text;
                                }
                            }
                        }
                    }

                    SendMsgEvent(this, new MyEventArg() { Value = valueDic });
                }
            }
            catch (Exception)
            {
                MessageBox.Show("输入的值不符合要求");
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SendMessage();
            this.FindForm().Close();
        }

        private void Level9Format_SelectedIndexChanged(object sender, EventArgs e)
        {
            string format = level9Format.Text;
            if (format == "ASCII" || format == "JIS8")
            {
                level9CheckBox.Text = "Value is binary";
            }
            else
            {
                level9CheckBox.Text = "Value is array";
            }
        }
    }
}
