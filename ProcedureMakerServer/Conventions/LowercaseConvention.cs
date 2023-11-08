using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ProcedureMakerServer;

public class LowercaseControllerModelConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Convert the controller's route template to lowercase// Convert the controller's route template to lowercase
        for (int i = 0; i < controller.Selectors.Count; i++)
        {
            var selector = controller.Selectors[i];
            selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.ToLower();

            //   controller.ControllerName = controller.ControllerName.ToLower();
            Console.WriteLine(controller.ControllerName); // works :)


        }

        for (int i = 0; i < controller.Actions.Count; i++)
        {
            var action = controller.Actions[i];
            action.ActionName = action.ActionName.ToLower();
            Console.WriteLine(action.ActionName);

        }

        //foreach (var item in controller.Properties.)
        //{

        //}
    }
}
