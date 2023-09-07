<template>
  <suspense>
    <q-page class="row items-center justify-evenly">
      <example-component
        title="Example component"
        active
        :todos="todos"
        :meta="meta"
      ></example-component>
    </q-page>
  </suspense>
</template>

<script lang="ts">
import { User, Todo, Meta } from 'components/models';
import ExampleComponent from 'components/ExampleComponent.vue';
import { defineComponent, ref } from 'vue';
import { AppVisibility, useQuasar } from 'quasar';
import { api } from 'boot/axios';
import { data } from 'autoprefixer';

export default defineComponent({
  name: 'IndexPage',
  components: { ExampleComponent },
  async setup() {
    const todos = ref<Todo[]>([
      {
        id: 1,
        content: 'item one',
      },
      {
        id: 2,
        content: 'ct2',
      },
      {
        id: 3,
        content: 'ct3',
      },
      {
        id: 4,
        content: 'ct4',
      },
      {
        id: 5,
        content: 'ct5',
      },
    ]);
    const $q = useQuasar();

    console.log('test');
    //let users = async() => await api.get<User[]>('/api/User').data;
    const users = await api.get<User[]>('/api/User').data;
    console.log('xprinting users no length...');
    console.log(users[0]);
    for (var i = 0; i < users.length; i++) {
      const user = users[i];
      console.log('Response: ' + user.Name);
      $q.notify({
        color: 'negative',
        position: 'top',
        message: users[i].Name,
        icon: 'report_problem',
      });
    }

    const meta = ref<Meta>({
      totalCount: 1200,
    });
    return { todos, meta };
  },
});
</script>
