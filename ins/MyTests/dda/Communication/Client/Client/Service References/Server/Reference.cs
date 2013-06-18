﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.Server {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Server.IServer", CallbackContract=typeof(Client.Server.IServerCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Register", ReplyAction="http://tempuri.org/IServer/RegisterResponse")]
        bool Register(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/sendMessageToServer", ReplyAction="http://tempuri.org/IServer/sendMessageToServerResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<object>))]
        System.Collections.Generic.List<object> sendMessageToServer(string depth, string postAttribute, string value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/sendMessageToClient", ReplyAction="http://tempuri.org/IServer/sendMessageToClientResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<object>))]
        System.Collections.Generic.List<object> sendMessageToClient(string depth, string postAttribute, string value);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerChannel : Client.Server.IServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerClient : System.ServiceModel.DuplexClientBase<Client.Server.IServer>, Client.Server.IServer {
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Register(string name) {
            return base.Channel.Register(name);
        }
        
        public System.Collections.Generic.List<object> sendMessageToServer(string depth, string postAttribute, string value) {
            return base.Channel.sendMessageToServer(depth, postAttribute, value);
        }
    }
}