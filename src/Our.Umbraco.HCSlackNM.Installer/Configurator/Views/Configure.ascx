<%@ Control Language="c#" AutoEventWireup="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script src="<%=ResolveUrl("~/umbraco/lib/angular/1.1.5/angular.min.js") %>"></script>
<script src="<%=ResolveUrl("~/App_Plugins/HealthCheckSlackNotificationMethod/Install/Configurator/Controllers/Configure.js") %>"></script>

<div ng-app ="UFSPLoader">
    <div ng-controller="Loader">
        <div class="row">
            <div class="span1">
                <img src="/App_Plugins/HealthCheckSlackNotificationMethod/Install/azure-logo-32.png"/>
            </div>
            <div><h4>Umbraco Slack Heath Check Notification Method</h4></div>
        </div>
        <div class="row">
            <div><hr /></div>
        </div>
        <div class="row" ng-show="!saved">
            <div>
                <fieldset>
                    <legend><h4>To complete installation, please enter the required settings</h4></legend>
                    <form name="paramForm" class="form-horizontal" role="form">
                        <div ng-repeat="param in parameters" class="control-group">
                            <ng-form name="form">
                            <label class="control-label" for="param.key">{{ capitalizeFirstLetter(param.key) }}</label>
                                <div class="controls">
                                    <span ng-if="getInputType(param.key) === 'checkbox'" ng-include="'<%=ResolveUrl("/App_Plugins/HealthCheckSlackNotificationMethod/Install/Configurator/Views/checkbox.htm") %>'"></span>
                                    <span ng-if="getInputType(param.key) === 'text'" ng-include="'<%=ResolveUrl("/App_Plugins/HealthCheckSlackNotificationMethod/Install/Configurator/Views/textfield.htm") %>'"></span>
                                </div>
                                <span data-ng-show=" {{'form.'+param.key+'.$dirty && form.'+param.key+'.$error.required'}}">Required!</span>
                            </ng-form>
                        </div>
                        <button preventDefault class="btn btn-primary" ng-disabled="paramForm.$invalid" ng-click="submitForm($event)">Save</button>
                    </form>
                </fieldset>
            </div>
        </div> 
        <div class="row" ng-show="!saved">
            <div><hr /></div>
         </div>

        <div class="row">
            <div>
                <div class="alert alert-success" ng-show="saved && (status === 'Ok')">
                  The Umbraco Slack Heath Check Notification Method was sucessfully configured
                </div>
            </div>
        </div>
           
    </div>
</div>

