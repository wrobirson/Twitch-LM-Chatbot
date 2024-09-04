export function PermissionsHelp() {
    return <div>
        <p className={'mb-2'}>
            <strong>Unrestricted (Sin restricciones):</strong>
            Al seleccionar esta opción, se permite el acceso sin ninguna restricción, lo que significa que cualquier
            usuario puede utilizar la funcionalidad asociada.
        </p>
        <p className={'mb-2'}>
            <strong>Followers (Seguidores):</strong>
            Esta opción restringe el acceso únicamente a los usuarios que siguen el canal o cuenta asociada. Solo
            los seguidores podrán utilizar la funcionalidad.
        </p>
        <p className={'mb-2'}>
            <strong>Subscribers (Suscriptores):</strong>
            Al seleccionar esta opción, el acceso queda restringido exclusivamente a los suscriptores del canal. Los
            usuarios que no sean suscriptores no podrán utilizar la funcionalidad.
        </p>
        <p className={'mb-2'}>
            <strong>Moderators (Moderadores):</strong>
            Esta opción restringe el acceso solo a los moderadores del canal. Los moderadores son los únicos que
            podrán utilizar la funcionalidad asociada.
        </p>

        <p className={'mb-2'}>
            <strong>Whitelist (Lista Blanca):</strong>
            Esta sección permite gestionar una lista de usuarios o elementos que tienen acceso permitido a la
            funcionalidad, independientemente de otras restricciones. Los usuarios o elementos pueden ser agregados
            a esta lista, y cada uno puede ser removido mediante un botón de eliminación.
        </p>
        <p className={'mb-2'}>
            <strong>Blacklist (Lista Negra):</strong>
            Esta sección permite gestionar una lista de usuarios o elementos que tienen acceso denegado a la
            funcionalidad, independientemente de otras configuraciones de permisos. Los usuarios o elementos pueden
            ser agregados a esta lista, y cada uno puede ser removido mediante un botón de eliminación.
        </p>

    </div>
}