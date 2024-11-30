using UnityEngine;

public static class GetcomponentExtension
{
    // Unified method to get components or interfaces from self, parent, or child
    public static T GetComponentExtend<T>
        (this GameObject obj, bool checkSelf = true, bool checkParent = true, bool checkChild = true) where T : class
    {
        // Check if T is a Unity Component
        if (typeof(Component).IsAssignableFrom(typeof(T)))
        {
            // Check Self if requested
            if (checkSelf)
            {
                Component component = obj.GetComponent(typeof(T));
                if (component != null)
                    return component as T;
            }

            // Check Parent if requested
            if (checkParent)
            {
                Component component = obj.GetComponentInParent(typeof(T));
                if (component != null)
                    return component as T;
            }

            // Check Child if requested
            if (checkChild)
            {
                Component component = obj.GetComponentInChildren(typeof(T));
                if (component != null)
                    return component as T;
            }
        }
        else
        {
            // Handle Interfaces or other reference types

            // Check Self if requested
            if (checkSelf)
            {
                T component = obj.GetComponent<T>();
                if (component != null)
                    return component;
            }

            // Check Parent if requested
            if (checkParent)
            {
                T component = obj.GetComponentInParent<T>();
                if (component != null)
                    return component;
            }

            // Check Child if requested
            if (checkChild)
            {
                T component = obj.GetComponentInChildren<T>();
                if (component != null)
                    return component;
            }
        }

        return null; // Return null if no component is found
    }
}
